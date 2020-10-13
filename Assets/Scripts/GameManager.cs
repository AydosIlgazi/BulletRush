using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    private int currentLevel;

    [SerializeField] GameObject EnemySpawner=default;
    [SerializeField] GameObject levelPanel = default;
    GameObject leftSpawner;
    GameObject topSpawner;
    GameObject rightSpawner;
    GameObject bottomSpawner;
    [SerializeField] public GameObject Player;
    PlayerUpgrades playerUpgrades;

    public int GameLevel { get; private set; }
    public int SpawnCountPerWave { get; private set; }
    public int EnemyCountPerSpawn { get; private set; }
    public int MaximumEnemySpawnedSimultenous { get; private set; }
    public int BulletDamage { get; set; }
    public int EnemyHealth { get; private set; }
    public float BulletSpeed { get; set; }
    public float BulletPenetration { get; set; }
    public float FireRate { get; set; }
    public float EnemyAttackRange { get; private set; }
    public float EnemySpeed { get; private set; }
    public float PlayerAttackRange { get; private set; }

    public static GameManager Instance
    {
        get { return _instance; }
    }

    void Start()
    {
        playerUpgrades = levelPanel.GetComponent<PlayerUpgrades>();
        StartLevel();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        SpawnCountPerWave = 2;
        EnemyHealth = 10;
        EnemySpeed = 1.5f;
        BulletDamage = 5;
        BulletSpeed = 20f;
        FireRate = 0.6f;
        GameLevel = 1;
        EnemyCountPerSpawn = 16;
        MaximumEnemySpawnedSimultenous = 9;
        EnemyAttackRange = 15f;
        PlayerAttackRange = 13f;
        currentLevel = 0;
        DontDestroyOnLoad(this.gameObject);
    }

    private void SpawnEnemies()
    {
        Vector3 spawnerLocation = new Vector3(-20f, 6f, Random.Range(-18f, 18f));//Left Spawner
        leftSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        leftSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.leftSpawner;

        spawnerLocation.Set(Random.Range(-18f, 18f), 6f, 20f);//Top Spawner
        topSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        topSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.topSpawner;

        spawnerLocation.Set(20f, 6f, Random.Range(-18f, 18f)); //Right Spawner
        rightSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        rightSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.rightSpawner;

        spawnerLocation.Set(Random.Range(-18f, 18f), 6f, -20f); //Botton spawner
        bottomSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        bottomSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.bottomSpawner;
    }

    private void DestroySpawners()
    {
        Destroy(leftSpawner);
        Destroy(rightSpawner);
        Destroy(topSpawner);
        Destroy(bottomSpawner);
    }

    IEnumerator EndLevelCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        levelPanel.SetActive(true);
        Player.transform.position = Vector3.zero;
        Player.GetComponent<Player>().KillAllEnemies();
        Player.SetActive(false);
        DestroySpawners();


    }

    public void StartLevel()
    {
        currentLevel++;
        levelPanel.SetActive(false);
        Player.SetActive(true);
        SpawnEnemies();
    }

    public void EndLevel()
    {
        playerUpgrades.SpendablePoints += 2;
        playerUpgrades.ShowStartLevel();
        playerUpgrades.ShowRevert();
        playerUpgrades.HideRestart();
        Player.GetComponent<Player>().forceShield.transform.localScale = new Vector3(3f, 0.1f, 3f);
        Player.GetComponent<Player>().isScaling = false;
        StartCoroutine(EndLevelCoroutine(1.5f));
        EnemyHealth += 5;
        EnemyCountPerSpawn += 1;
        EnemyAttackRange += 0.5f;
        EnemySpeed += 0.15f;
        if (currentLevel % 5 == 0)
        {
            playerUpgrades.SpendablePoints += 3;
            SpawnCountPerWave += 1;
        }
    }

    public void RestartLevel()
    {
        levelPanel.SetActive(false);
        Player.SetActive(true);
        Player.GetComponent<Player>().forceShield.transform.localScale = new Vector3(3f, 0.1f, 3f);
        Player.GetComponent<Player>().isScaling = false;
        SpawnEnemies();
    }

    public void PlayerDied()
    {
        playerUpgrades.HideRevert();
        playerUpgrades.HideStartLevel();
        playerUpgrades.ShowRestart();
        StartCoroutine(EndLevelCoroutine(0));

    }
}
