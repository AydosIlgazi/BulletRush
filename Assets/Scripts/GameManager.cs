using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public int GameLevel { get; private set; }
    public int SpawnCountPerWave { get; private set; }

    public int EnemyCountPerSpawn { get; private set; }
    public int MaximumEnemySpawnedSimultenous { get; private set; }
    public int EnemyHealth { get; private set; }

    [SerializeField] GameObject EnemySpawner=default;
    [SerializeField] public GameObject User;

    public static GameManager Instance
    {
        get { return _instance; }
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        Debug.Log("Awake");
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        SpawnCountPerWave = 4;
        EnemyHealth = 20;
        GameLevel = 1;
        EnemyCountPerSpawn = 5;
        MaximumEnemySpawnedSimultenous = 11;
        DontDestroyOnLoad(this.gameObject);
        StartLevel();
    }

    public void StartLevel()
    {
        Vector3 spawnerLocation = new Vector3(-20f, 6f, Random.Range(-18f, 18f));//Left Spawner
        GameObject leftSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        leftSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.leftSpawner;

        spawnerLocation.Set(Random.Range(-18f, 18f), 6f, 20f);//Top Spawner
        GameObject topSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        topSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.topSpawner;

        spawnerLocation.Set(20f, 6f, Random.Range(-18f, 18f)); //Right Spawner
        GameObject rightSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        rightSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.rightSpawner;

        spawnerLocation.Set(Random.Range(-18f, 18f), 6f, -20f); //Botton spawner
        GameObject bottomSpawner= Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        bottomSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.bottomSpawner;
    }

    public void EndLevel()
    {

    }
}
