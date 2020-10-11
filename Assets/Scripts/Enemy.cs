using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector3 direction;
    GameManager instance;

    public float movementSpeed= 0.2f;
    public Vector3 target;
    public Rigidbody rb;
    public GameObject user;
    public GameObject gameStatus;
    public int enemyHealth;
    
    void Awake()
    {
        instance = GameManager.Instance;
        movementSpeed = instance.EnemySpeed;
        enemyHealth = instance.EnemyHealth;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //if player is in enemy attack range, follow player
        if ((transform.position - instance.Player.transform.position).magnitude< instance.EnemyAttackRange){
            direction = (instance.Player.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
        }
        //Move towards center of the map
        else
        {
            target = new Vector3(Random.Range(-10f, 10f), transform.position.y, Random.Range(-10f, 10f));
            direction = (target - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            enemyHealth -= collision.gameObject.GetComponent<Bullet>().bulletDamage;
            if (enemyHealth <= 0)
            {
                Vector3 forceShieldScale = instance.Player.GetComponent<Player>().ForceShield.transform.localScale;
                instance.Player.GetComponent<Player>().ForceShield.transform.localScale = new Vector3(forceShieldScale.x + 0.5f, forceShieldScale.y, forceShieldScale.z + 0.5f);
                instance.Player.GetComponent<Player>().RemoveEnemy(gameObject.GetComponent<Enemy>());
            }
        }
    }
}
