using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Targetable {LeftTargetable,RightTargetable };
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
    public GameObject leftEnemySearcher;
    public GameObject rightEnemySearcher;
    public GameObject enemyTemp;
    public List<Enemy> enemyList;
    private float maxY=75f;
    private float minY=10f;
    private float maxZ=45f;   //In z Rotation Openin arm reverse transform
    private float minZ=-20f;
    private float armClosingOffset = 10f;
    private float yOffset;

    void Awake()
    {
        enemyList = new List<Enemy>();
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {

        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        animator.SetFloat("speed", rb.velocity.magnitude);
        //if(direction != Vector3.zero)
        //{
        //    Vector3 lookDirection = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);
        //    Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        //    float step = rotationSpeed * Time.deltaTime;
        //    transform.rotation = Quaternion.RotateTowards(lookRotation, transform.rotation, step);
        //}
        

    }

    void LateUpdate()
    {
        Enemy leftClosestEnemy = GetClosestEnemy(enemyList, leftEnemySearcher);
        Enemy rightClosestEnemy = GetClosestEnemy(enemyList, rightEnemySearcher);
        float distanceLeft = (leftClosestEnemy.transform.position - leftWrist.transform.position).magnitude;
        float distanceRight = (rightClosestEnemy.transform.position - rightWrist.transform.position).magnitude;
        if(distanceLeft<= GameManager.Instance.PlayerAttackRange|| distanceRight <= GameManager.Instance.PlayerAttackRange)
        {
            //Rotate towards to enemy

            Vector3 leftEnemyDirection = (leftClosestEnemy.transform.position - transform.position).normalized; //Enemy to Kyle
            Quaternion lookLeftRotationEnemy = Quaternion.LookRotation(leftEnemyDirection, Vector3.up);
            float playerYLeftTransform=Quaternion.RotateTowards(lookLeftRotationEnemy, transform.rotation, rotationSpeed * Time.deltaTime).eulerAngles.y; //Player Y transform value needded to look towards enemy
            bool isLeftTargetable= CheckTargetable(playerYLeftTransform,Targetable.LeftTargetable);

            Vector3 rightEnemyDirection = (rightClosestEnemy.transform.position - transform.position).normalized; //Enemy to Kyle
            Quaternion lookRightRotationEnemy = Quaternion.LookRotation(rightEnemyDirection, Vector3.up);
            float playerYRightTransform = Quaternion.RotateTowards(lookRightRotationEnemy, transform.rotation, rotationSpeed * Time.deltaTime).eulerAngles.y; //Player Y transform value needded to look towards enemy
            bool isRightTargetable = CheckTargetable(playerYRightTransform, Targetable.RightTargetable);

            if(isLeftTargetable && isRightTargetable) // both arms can shoot different enemies 
            {
                Debug.Log("Both Targerable");
                float leftArmZRotation = YZRotationMapper(playerYLeftTransform);
                float rightArmZRotation = YZRotationMapper(playerYRightTransform);
                leftArm.transform.localRotation = Quaternion.Euler(new Vector3(-165f, 90f, leftArmZRotation));
                rightArm.transform.localRotation = Quaternion.Euler(new Vector3(15f, 90f, rightArmZRotation));
            }
            else
            {
                Enemy closestEnemy;
                if (distanceLeft < distanceRight)
                    closestEnemy = leftClosestEnemy;
                else
                    closestEnemy = rightClosestEnemy;

                Vector3 enemyDirection = (closestEnemy.transform.position - transform.position).normalized;
                Quaternion lookRotationEnemy = Quaternion.LookRotation(enemyDirection, Vector3.up);
                transform.rotation = Quaternion.Euler(new Vector3(0f, Quaternion.RotateTowards(lookRotationEnemy, transform.rotation, rotationSpeed * Time.deltaTime).eulerAngles.y));
                leftArm.transform.localRotation = Quaternion.Euler(new Vector3(-165f, 90f, 45f));
                rightArm.transform.localRotation = Quaternion.Euler(new Vector3(15f, 90f, 45f));
            }
        }
        else
        {
            //Rotate with joystick
            Vector3 directionJoystick = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            if (directionJoystick != Vector3.zero)
            {
                Vector3 lookDirection = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);
                Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

                float step = rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(lookRotation, transform.rotation, step);

            }
        }




        float stepEnemy = rotationSpeed * Time.deltaTime;
        Vector3 direction = (enemyTemp.transform.position - transform.position).normalized;
        //Quaternion lookRotationEnemy = Quaternion.LookRotation(direction,Vector3.up);
        //Debug.Log(Quaternion.RotateTowards(lookRotationEnemy, transform.rotation, stepEnemy).eulerAngles.y);
        //transform.rotation= Quaternion.RotateTowards(lookRotationEnemy, transform.rotation, stepEnemy);
        //float stepEnemy = rotationSpeed * Time.deltaTime;
        //Vector3 direction = (enemyTemp.transform.position - leftArm.transform.position).normalized;
        //Quaternion lookRotationEnemy = Quaternion.LookRotation(direction);
        //Debug.Log(lookRotationEnemy.eulerAngles.y);
        //lookRotationEnemy = Quaternion.Euler(-165f, 90f,Mathf.Clamp(lookRotationEnemy.eulerAngles.y ,-60f,30f) );
        //Debug.Log(lookRotationEnemy.z);

        //leftArm.transform.localRotation = Quaternion.RotateTowards(lookRotationEnemy, leftArm.transform.localRotation, stepEnemy);
        //leftArm.transform.localRotation = Quaternion.Euler(new Vector3(-165f, 90f, 30f));
        //rightArm.transform.localRotation = Quaternion.Euler(new Vector3(15f, 90f, 30f));
        //transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.y, transform.rotation.z));
        leftForeArm.transform.localRotation = Quaternion.Euler(new Vector3(0f, 15f, -55f));
        rightForeArm.transform.localRotation = Quaternion.Euler(new Vector3(110f, -35f, -50f));
        upperLeftArm.transform.localRotation = Quaternion.Euler(new Vector3(10f, 7f, 50f));
        upperRightArm.transform.localRotation = Quaternion.Euler(new Vector3(-2f, -7f, 40f));
        leftWrist.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 55f));
        rightWrist.transform.localRotation = Quaternion.Euler(new Vector3(-90f, -90f, 85f ));
    }

    private bool CheckTargetable(float yTransform,Targetable targetable)
    {
        float playerYAbs = transform.rotation.eulerAngles.y; //Get player's current rotation
        bool isCloserDegreeExist = Mathf.Abs(playerYAbs - yTransform) > 180; //find closest degree between player's current and targeted enemy
        if (playerYAbs > 180 && isCloserDegreeExist)
            playerYAbs = playerYAbs - 360;
        if (yTransform > 180 && isCloserDegreeExist)
            yTransform = yTransform - 360;
        yOffset = Math.Abs (yTransform - playerYAbs); //How much rotation needed to hit the enemy
        Debug.Log(targetable + "  /  " +yTransform+"  /  "+ transform.rotation.eulerAngles.y + "  /  " + yOffset);
        if(targetable == Targetable.LeftTargetable)
        {
            if (yOffset < 65 &&  yTransform<= playerYAbs ) //robot's arm can rotate maximum 65 degrees check if it can hit that object
                return true;
            else
                return false;
        }
        else
        {
            if (yOffset < 65 && yTransform > playerYAbs) // for right arm
                return true;
            else
                return false;
        }
    }

    private Enemy GetClosestEnemy(List<Enemy> enemies, GameObject player)
    {
        Enemy bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;
        foreach (Enemy potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
    
    private float YZRotationMapper(float yTransform)
    {
        //We know enemy hittable so calculate the rotation degree of arm
        float yDifference = maxY - yOffset; //Calculate in y axis
        float correspondingZ = yDifference + minZ; //Map to Z axis
        correspondingZ = Mathf.Clamp(correspondingZ, minZ, maxZ);  // We dont want to arms broke :) 
        return correspondingZ;
    }


    public void AddEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }


}