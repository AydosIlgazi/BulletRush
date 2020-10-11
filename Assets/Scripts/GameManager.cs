using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    private int currentLevel;

    [SerializeField] GameObject EnemySpawner=default;
    [SerializeField] GameObject levelPanel = default;
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
        EnemySpeed = 0.5f;
        BulletDamage = 5;
        BulletSpeed = 4f;
        FireRate = 1f;
        GameLevel = 1;
        EnemyCountPerSpawn = 16;
        MaximumEnemySpawnedSimultenous = 9;
        EnemyAttackRange = 12f;
        PlayerAttackRange = 10f;
        currentLevel = 0;
        DontDestroyOnLoad(this.gameObject);
    }

    private void SpawnEnemies()
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
        GameObject bottomSpawner = Instantiate(EnemySpawner, spawnerLocation, Quaternion.identity);
        bottomSpawner.GetComponent<EnemySpawner>().spawnerType = SpawnerType.bottomSpawner;
    }

    IEnumerator EndLevelCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        levelPanel.SetActive(true);
        Player.transform.position = Vector3.zero;
        Player.GetComponent<Player>().KillAllEnemies();
        Player.SetActive(false);

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
        playerUpgrades.SpendablePoints += 1;
        playerUpgrades.ShowStartLevel();
        playerUpgrades.ShowRevert();
        playerUpgrades.HideRestart();
        StartCoroutine(EndLevelCoroutine(1.5f));
        EnemyHealth += 5;
        EnemyCountPerSpawn += 1;
        EnemyAttackRange += 0.5f;
        EnemySpeed += 0.15f;
        if (currentLevel % 5 == 0)
        {
            playerUpgrades.SpendablePoints += 1;
            SpawnCountPerWave += 1;
        }
    }

    public void RestartLevel()
    {
        levelPanel.SetActive(false);
        Player.SetActive(true);
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
