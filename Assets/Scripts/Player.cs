using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    public float rotationSpeed;
    Animator animator;
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject leftForeArm;
    public GameObject rightForeArm;
    public GameObject upperRightArm;
    public GameObject upperLeftArm;
    public GameObject leftWrist;
    public GameObject rightWrist;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        animator.SetFloat("speed", rb.velocity.magnitude);
        if(direction != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(lookRotation, transform.rotation, step);
        }
    }

    public void LateUpdate()
    {
        leftArm.transform.localRotation = Quaternion.Euler(new Vector3(-165f, 90f, 30f));
        rightArm.transform.localRotation = Quaternion.Euler(new Vector3(15f, 90f, 30f));
        leftForeArm.transform.localRotation = Quaternion.Euler(new Vector3(0f, 15f, -55f));
        rightForeArm.transform.localRotation = Quaternion.Euler(new Vector3(110f, -35f, -50f));
        upperLeftArm.transform.localRotation = Quaternion.Euler(new Vector3(10f, 7f, 50f));
        upperRightArm.transform.localRotation = Quaternion.Euler(new Vector3(-2f, -7f, 40f));
        leftWrist.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 55f));
        rightWrist.transform.localRotation = Quaternion.Euler(new Vector3(-90f, -90f, 85f ));
    }
}