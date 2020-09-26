using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    public float rotationSpeed;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        Debug.Log(direction);
        if(direction != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(lookRotation, transform.rotation, step);
        }

    }
}