using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float bulletPenetration;
    private bool isPenetrated = false;

    BulletPool bulletPool;
    Rigidbody rb;
    GameManager gameManager;

    public PoolType bulletType;
    public int bulletDamage;
    // Start is called before the first frame update

    void Awake()
    {
       
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        bulletPool = BulletPool.instance;
        gameManager = GameManager.Instance;
        bulletDamage = gameManager.BulletDamage;
        bulletPenetration = gameManager.BulletPenetration;
    }

    void FixedUpdate()
    {
        float speed = gameManager.BulletSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction * speed);
        if (transform.position.z > 100)
        {
            bulletPool.BackToPool(PoolType.regulerBullet, this.gameObject);
        }
    }

    void OnEnable()
    {
        direction = transform.rotation * Vector3.forward;
        direction.y = 0f;
        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var enemyHealth = collision.gameObject.GetComponent<Enemy>().enemyHealth;
            if(bulletDamage > enemyHealth)
            {
                bulletDamage = bulletDamage - enemyHealth;
            }
            else
            {
                if (!isPenetrated)
                {
                    bulletDamage = Mathf.RoundToInt(bulletDamage * bulletPenetration / 100);
                    isPenetrated = true;
                }
                else
                {
                    bulletPool.BackToPool(PoolType.regulerBullet, this.gameObject);

                }
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            bulletPool.BackToPool(PoolType.regulerBullet, this.gameObject);
        }

    }
}
