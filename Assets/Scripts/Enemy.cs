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
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((transform.position - GameManager.Instance.User.transform.position).magnitude<12){
            direction = (GameManager.Instance.User.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
        }
        else
        {
            target = new Vector3(Random.Range(-5f, 5f), transform.position.y, Random.Range(-5f, 5f));
            direction = (target - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
        }

    }
}
