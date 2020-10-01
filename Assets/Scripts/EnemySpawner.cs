using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpawnerType { topSpawner, rightSpawner, bottomSpawner,leftSpawner };

public class EnemySpawner : MonoBehaviour
{
    private Vector3 offSetHorizontalRight;
    private Vector3 offSetHorizontalLeft;
    private Vector3 offSetVerticalUp;
    private Vector3 offSetVerticalDown;
    private Vector3 yCoordFixer;
    private float offSet = 0.25f;
    [SerializeField] GameObject Enemy=default;
    public SpawnerType spawnerType;
    // Start is called before the first frame update
    void Start()
    {
        offSetHorizontalRight = new Vector3(offSet, 0f, 0f);
        offSetHorizontalLeft = new Vector3(-offSet, 0f, 0f);
        offSetVerticalUp = new Vector3(0f, 0f, offSet);
        offSetVerticalDown = new Vector3(0f, 0f, -offSet);
        yCoordFixer = new Vector3(0f, -5.5f, 0f);
        for (int i = 0; i < GameManager.Instance.SpawnCountPerWave; i++)
        {
            //StartCoroutine(InstantiateEnemies());
        }
        StartCoroutine(InstantiateEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator InstantiateEnemies()
    {

        for (int i = 0; i < 1; i++)
        {
            Instantiate(Enemy, transform);
            int k = 1;
            Vector3 topScale = transform.position + yCoordFixer; ;
            Vector3 rightScale = transform.position+yCoordFixer;
            Vector3 leftScale = transform.position+yCoordFixer;
            Vector3 bottomScale = transform.position+yCoordFixer;
            for (int j = 0; j < GameManager.Instance.EnemyCountPerSpawn; j++)
            {
                if (k + 1 > GameManager.Instance.MaximumEnemySpawnedSimultenous)
                {
                    k = 1;
                    topScale.z += offSetVerticalUp.z;
                    rightScale.x += offSetHorizontalRight.x;
                    leftScale.x += offSetHorizontalLeft.x;
                    bottomScale.z += offSetVerticalDown.z;
                }
                if (spawnerType == SpawnerType.topSpawner)
                {
                    Instantiate(Enemy, topScale + (offSetHorizontalRight * (k)), Quaternion.identity, transform);
                    Instantiate(Enemy, topScale + (offSetHorizontalLeft * (k)), Quaternion.identity, transform);
                }
                else if (spawnerType == SpawnerType.rightSpawner)
                {
                    Instantiate(Enemy, rightScale + (offSetVerticalUp * (k)), Quaternion.identity, transform);
                    Instantiate(Enemy, rightScale + (offSetVerticalDown * (k)), Quaternion.identity, transform);
                }
                else if (spawnerType == SpawnerType.leftSpawner)
                {
                    Instantiate(Enemy, leftScale + (offSetVerticalUp * (k)), Quaternion.identity, transform);
                    Instantiate(Enemy, leftScale + (offSetVerticalDown * (k)), Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(Enemy, bottomScale + (offSetVerticalUp * (k)), Quaternion.identity, transform);
                    Instantiate(Enemy, bottomScale + (offSetVerticalDown * (k)), Quaternion.identity, transform);
                }
                k++;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
