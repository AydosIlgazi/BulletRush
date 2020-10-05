using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed= 0.2f;
    public Vector3 target;
    public Rigidbody rb;
    public GameObject user;
    Vector3 direction;
    public GameObject gameStatus;
    GameManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = GameManager.Instance;
    }
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if player is in enemy attack range, follow player
        if ((transform.position - instance.User.transform.position).magnitude< instance.EnemyAttackRange){
            direction = (instance.User.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
        }
        //Move towards center of the map
        else
        {
            target = new Vector3(Random.Range(-5f, 5f), transform.position.y, Random.Range(-5f, 5f));
            direction = (target - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
        }

    }
}
