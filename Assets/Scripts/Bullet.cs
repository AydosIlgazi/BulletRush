using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    BulletPool bulletPool;
    Rigidbody rb;
    // Start is called before the first frame update

    void Awake()
    {
       
        bulletPool = BulletPool.instance;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    void OnEnable()
    {
        direction = transform.rotation * Vector3.forward;
        direction.y = 0f;
        Debug.Log(direction);
        transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // Debug.Log(direction);
        float speed = 1f * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + direction *speed );
        if (transform.position.z > 100)
        {
            bulletPool.BackToPool("bullet", this.gameObject);
        }
    }
}
