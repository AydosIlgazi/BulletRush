using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum Targetable {LeftTargetable,RightTargetable };
public class Player : MonoBehaviour
{
    private float maxY = 60f;
    private float maxZ = 39f;   //In z Rotation Opening arm reverse transform
    private float minZ = -20f;
    private float yOffset;
    private float fireTimer = 0f;
    private PoolType currentBulletType;
    private bool isScaling = false;

    Animator animator;
    GameManager gameManager;
    BulletPool pool;
    [SerializeField] GameObject enemiesLeftText=default;

    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    public float rotationSpeed;
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
    public GameObject leftGun;
    public GameObject rightGun;
    public GameObject fireStarterLeft;
    public GameObject fireStarterRigth;
    public GameObject forceShield;
    public GameObject enemyKillEffect;
    public List<Enemy> enemyList;

    void Awake()
    {
        enemyList = new List<Enemy>();
        currentBulletType = PoolType.regulerBullet;
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        gameManager = GameManager.Instance;
        pool = BulletPool.instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TriggerShield();
        }
    }

    void FixedUpdate()
    {


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
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        animator.SetFloat("speed", rb.velocity.magnitude);
        if (enemyList.Count > 0)
        {
            //Find left and right cloesest enemies
            Enemy leftClosestEnemy = GetClosestEnemy(enemyList, leftEnemySearcher);
            Enemy rightClosestEnemy = GetClosestEnemy(enemyList, rightEnemySearcher);
            float distanceLeft = (leftClosestEnemy.transform.position - leftWrist.transform.position).magnitude;
            float distanceRight = (rightClosestEnemy.transform.position - rightWrist.transform.position).magnitude;

            ////Override Arm Animation
            leftForeArm.transform.localRotation = Quaternion.Euler(new Vector3(0f, 15f, -55f));
            rightForeArm.transform.localRotation = Quaternion.Euler(new Vector3(110f, -35f, -50f));
            upperLeftArm.transform.localRotation = Quaternion.Euler(new Vector3(10f, 7f, 49f));
            upperRightArm.transform.localRotation = Quaternion.Euler(new Vector3(-2f, -7f, 49f));
            leftWrist.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 55f));
            rightWrist.transform.localRotation = Quaternion.Euler(new Vector3(-90f, -90f, 85f));


            if ((distanceLeft <= gameManager.PlayerAttackRange || distanceRight <= gameManager.PlayerAttackRange) && !animator.GetBool("isForceShield"))
            {

                //Rotate towards to enemy

                Vector3 leftEnemyDirection = (leftClosestEnemy.transform.position - transform.position).normalized; //Enemy to Player
                Quaternion lookLeftRotationEnemy = Quaternion.LookRotation(leftEnemyDirection, Vector3.up);
                float playerYLeftTransform = Quaternion.RotateTowards(lookLeftRotationEnemy, transform.rotation, rotationSpeed * Time.deltaTime).eulerAngles.y; //Player Y transform value needded to look towards enemy
                bool isLeftTargetable = CheckTargetable(playerYLeftTransform, Targetable.LeftTargetable);

                Vector3 rightEnemyDirection = (rightClosestEnemy.transform.position - transform.position).normalized; //Enemy to Player
                Quaternion lookRightRotationEnemy = Quaternion.LookRotation(rightEnemyDirection, Vector3.up);
                float playerYRightTransform = Quaternion.RotateTowards(lookRightRotationEnemy, transform.rotation, rotationSpeed * Time.deltaTime).eulerAngles.y; //Player Y transform value needded to look towards enemy
                bool isRightTargetable = CheckTargetable(playerYRightTransform, Targetable.RightTargetable);

                if (isLeftTargetable && isRightTargetable ) // both arms can shoot different enemies 
                {

                    float leftArmZRotation = YZRotationMapper(playerYLeftTransform);
                    float rightArmZRotation = YZRotationMapper(playerYRightTransform);

                    leftArm.transform.localRotation = Quaternion.Euler(new Vector3(-165f, 90f, leftArmZRotation));
                    rightArm.transform.localRotation = Quaternion.Euler(new Vector3(15f, 90f, rightArmZRotation));
                }
                else // only enemy from left or right can be shoot bcs robot can open arms up to some degree, shoot closest
                {
                    Enemy closestEnemy;
                    if (distanceLeft < distanceRight)
                        closestEnemy = leftClosestEnemy;
                    else
                        closestEnemy = rightClosestEnemy;

                    Vector3 enemyDirection = (closestEnemy.transform.position - transform.position).normalized;
                    Quaternion lookRotationEnemy = Quaternion.LookRotation(enemyDirection, Vector3.up);
                    transform.rotation = Quaternion.Euler(new Vector3(0f, Quaternion.RotateTowards(lookRotationEnemy, transform.rotation, rotationSpeed * Time.fixedDeltaTime).eulerAngles.y));
                    leftArm.transform.localRotation = Quaternion.Euler(new Vector3(-165f, 90f, 39f));
                    rightArm.transform.localRotation = Quaternion.Euler(new Vector3(15f, 90f, 39f));
                }
                fireTimer += Time.fixedDeltaTime;
                //Start fire
                if (fireTimer >= gameManager.FireRate)
                {
                    fireTimer = 0;
                    GameObject leftBullet = pool.Fire(currentBulletType, fireStarterLeft.transform.position, leftGun.transform.rotation);
                    GameObject rightBullet = pool.Fire(currentBulletType, fireStarterRigth.transform.position, rightGun.transform.rotation);


                }

            }
            else //normal movement
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
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Enemy")
        {
            gameManager.PlayerDied();
        }
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
        float playerYAbs = transform.rotation.eulerAngles.y; //Get player's current rotation
        bool isCloserDegreeExist = Mathf.Abs(playerYAbs - yTransform) > 180; //find closest degree between player's current and targeted enemy
        if (playerYAbs > 180 && isCloserDegreeExist)
            playerYAbs = playerYAbs - 360;
        if (yTransform > 180 && isCloserDegreeExist)
            yTransform = yTransform - 360;
        yOffset = Math.Abs(yTransform - playerYAbs); //How much rotation needed to hit the enemy
        //We know enemy hittable so calculate the rotation degree of arm
        float yDifference = maxY - yOffset; //Calculate in y axis
        float correspondingZ = yDifference + minZ; //Map to Z axis
        correspondingZ = Mathf.Clamp(correspondingZ, minZ, maxZ);  // We dont want to arms broke :) 
        return correspondingZ;
    }

    private void TriggerShield()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        var enemiesInsideShield = Physics.OverlapSphere(forceShield.transform.position, forceShield.GetComponent<Renderer>().bounds.extents.magnitude);
        StartCoroutine(ForceShieldTrigger(forceShield.transform.localScale.y, forceShield.transform.localScale.x, 0.7f));
        foreach(var gameObj in enemiesInsideShield)
        {
            
            if (gameObj.tag == "Enemy")
            {
                RemoveEnemy(gameObj.GetComponent<Enemy>(),0.5f);
            }
        }
        
    }

    IEnumerator ForceShieldTrigger(float fromVal, float toVal, float duration)
    {
        float counter = 0f;
        animator.SetBool("isForceShield", true);
        while (counter < duration)
        {

            counter += Time.unscaledDeltaTime;


            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            forceShield.transform.localScale = new Vector3(val, val, val);
            yield return null;
        }
        forceShield.transform.localScale = new Vector3(3f, 0.1f, 3f);

    }

    IEnumerator ForceShieldAreaIncrease(float duration)
    {

        if (isScaling)
        {
            yield break; ///exit if this is still running
        }
        isScaling = true;

        float counter = 0;

        Debug.Log("scaling");
        //Get the current scale of the object to be moved
        Vector3 startScaleSize = forceShield.transform.localScale;
        Vector3 toScale = new Vector3(startScaleSize.x + 0.5f, startScaleSize.y, startScaleSize.z + 0.5f);
        while (counter < duration)
        {
            counter += Time.fixedDeltaTime;
            Debug.Log(counter);
            Debug.Log(startScaleSize);
            Debug.Log(toScale);
            Debug.Log(counter / duration);
            Debug.Log(Vector3.Lerp(startScaleSize, toScale, counter / duration));
            forceShield.transform.localScale = Vector3.Lerp(startScaleSize, toScale, counter / duration);
            Debug.Log(forceShield.transform.localScale);
            yield return null;
        }
        isScaling = false;
    }

    public void AddEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
        enemiesLeftText.GetComponent<TextMeshProUGUI>().text = "Enemies left: " + enemyList.Count;
    }

    public void RemoveEnemy(Enemy enemy,float inTime=0)
    {
        if (enemyList.Count > 0)
        {
            if (Input.GetMouseButton(0) && forceShield.GetComponent<Renderer>().bounds.extents.magnitude<10)
            {
                Debug.Log(forceShield.GetComponent<Renderer>().bounds.extents.magnitude < 10);
                StartCoroutine(ForceShieldAreaIncrease(0.5f));
            }
            enemyList.Remove(enemy);
            Instantiate(enemyKillEffect, enemy.transform.position,enemy.transform.rotation);
            Destroy(enemy.gameObject,inTime);
            if (enemyList.Count == 0)
            {
                speed += 0.5f;
                gameManager.EndLevel();
            }
        }
        enemiesLeftText.GetComponent<TextMeshProUGUI>().text = "Enemies left: " + enemyList.Count;

    }

    public void ForceShieldAnimationHandler()
    {
        animator.SetBool("isForceShield", false);
        
    }

    public void KillAllEnemies()
    {
        foreach(Enemy enemy in enemyList.ToList())
        {
            enemyList.Remove(enemy);
            Destroy(enemy.gameObject, 0);
        }
    }


}