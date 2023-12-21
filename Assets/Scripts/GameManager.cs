using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject enemy1Prefeb, enemy2Prefeb;
    public GameObject goldCoinPrefeb, silverCoinPrefeb, healthBoxPrefeb;
    public Transform coinHealthBoxParent;
    public int enemyPoolSize = 15;
    public Transform enemyParent;
    private List<GameObject> enemy1 = new List<GameObject>();
    private List<GameObject> enemy2 = new List<GameObject>();
    private List<GameObject> goldCoinPool= new List<GameObject>();
    private List<GameObject>  silverCoinPool = new List<GameObject>();
    private List<GameObject> healthBoxPool = new List<GameObject>();
    public float spawnRaduis = 10;
    public Transform player;
    bool isSpawnEnemy;
    // bullet 
    public int bulletPoolSize = 10;
    public GameObject bulletPrefab;
    List<GameObject> bulletPool;
    public Transform bulletParent;
    public int level;
    float spawnTime = 1.5f;
    public GameObject restartScreen;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        restartScreen.SetActive(false);
        InitializeBulletPool();
        InitializeEnmeyPool();
        InitializeCoinsAndHelthBoxPool();
        player = GameObject.FindWithTag("Player").transform;

        

    }

    void InitializeCoinsAndHelthBoxPool()
    {
        for (int i = 0; i < enemyPoolSize / 3; i++)
        {
            // generate objects 
            GameObject goldCoin = Instantiate(goldCoinPrefeb);
            GameObject silverCoin = Instantiate(silverCoinPrefeb);
            GameObject healthBox = Instantiate(healthBoxPrefeb);
            // set parent 
            goldCoin.transform.parent = coinHealthBoxParent.transform;
            silverCoin.transform.parent = coinHealthBoxParent.transform;
            healthBox.transform.parent = coinHealthBoxParent.transform;
            // disable all
            goldCoin.SetActive(false);
            silverCoin.SetActive(false);
            healthBox.SetActive(false);
            // add into list
            goldCoinPool.Add(goldCoin);
            silverCoinPool.Add(silverCoin);
            healthBoxPool.Add(healthBox);
        }
    }

    public GameObject GetGoldCoin()
    {
        foreach (GameObject gCoin in goldCoinPool)
        {
            if (!gCoin.activeInHierarchy)
            {
                gCoin.SetActive(true);
                gCoin.transform.rotation = Quaternion.EulerAngles(Vector3.zero);
                return gCoin;
            }
        }

        GameObject newGCoin = Instantiate(goldCoinPrefeb);
        newGCoin.transform.parent = coinHealthBoxParent.transform;

        newGCoin.SetActive(true);
        goldCoinPool.Add(newGCoin);
        return newGCoin;

    }
    public GameObject GetSilverCoin()
    {
        foreach (GameObject sCoin in silverCoinPool)
        {
            if (!sCoin.activeInHierarchy)
            {
                sCoin.SetActive(true);
                return sCoin;
            }
        }

        GameObject newSCoin = Instantiate(silverCoinPrefeb);
        newSCoin.transform.parent = coinHealthBoxParent.transform;
        newSCoin.SetActive(true);
        silverCoinPool.Add(newSCoin);
        return newSCoin;

    }
    public GameObject GetHealthBoxCoin()
    {
        foreach (GameObject hBox in healthBoxPool)
        {
            if (!hBox.activeInHierarchy)
            {
                hBox.SetActive(true);
                return hBox;
            }
        }

        GameObject newHBox = Instantiate(healthBoxPrefeb);
        newHBox.transform.parent = coinHealthBoxParent.transform;
        newHBox.SetActive(true);
        goldCoinPool.Add(newHBox);
        return newHBox;

    }

    void InitializeEnmeyPool()
    {
        for (int i = 0; i < enemyPoolSize; i++)
        {
            GameObject enmy = Instantiate(enemy1Prefeb, Vector3.one * 10, Quaternion.identity);
            enmy.transform.parent = enemyParent.transform;
            enmy.SetActive(false);
            enemy1.Add(enmy);
        }

        for (int i = 0; i < enemyPoolSize / 2; i++)
        {
            GameObject enmy = Instantiate(enemy2Prefeb, Vector3.one * 10, Quaternion.identity);
            enmy.transform.parent = enemyParent.transform;
            enmy.SetActive(false);
            enemy2.Add(enmy);
        }
    }

    public GameObject GetEnemyTypeOne()
    {
        foreach (GameObject enmy in enemy1)
        {
            if (!enmy.activeInHierarchy)
            {
                // reset health
                var thisenemy = enmy.GetComponent<Enemy>();
                thisenemy.health = 100;
                thisenemy.OnInit();

                enmy.SetActive(true);
                return enmy;
            }
        }

        GameObject newEnmy = Instantiate(enemy1Prefeb, Vector3.one * 10, Quaternion.identity);
        newEnmy.transform.parent = enemyParent.transform;
        // reset health
        var enemy = newEnmy.GetComponent<Enemy>();
        enemy.health = 100;
        enemy.OnInit();
        newEnmy.SetActive(true);
        enemy1.Add(newEnmy);
        return newEnmy;

    }

    public GameObject GetEnemyTypeTwo()
    {
        foreach (GameObject enmy in enemy2)
        {
            if (!enmy.activeInHierarchy)
            {
                // reset health
                var thisenemy = enmy.GetComponent<Enemy>();
                thisenemy.health = 100;
                thisenemy.OnInit();

                enmy.SetActive(true);
                return enmy;
            }
        }

        GameObject newEnmy = Instantiate(enemy2Prefeb, Vector3.one * 10, Quaternion.identity);
        newEnmy.transform.parent = enemyParent.transform;
        // reset health
        var enemy = newEnmy.GetComponent<Enemy>();
        enemy.health = 100;
        enemy.OnInit();

        newEnmy.SetActive(true);
        enemy2.Add(newEnmy);
        return newEnmy;

    }

    void InitializeBulletPool()
    {
        bulletPool = new List<GameObject>();
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.parent = bulletParent;
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.parent = bulletParent;
        newBullet.SetActive(true);
        bulletPool.Add(newBullet);
        return newBullet;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawnEnemy && PlayerControler.Instance.health > 0)
            SpawnEnemies();
    }

    void SpawnEnemies()
    {

        isSpawnEnemy = true;
        Vector2 randomPoint = Random.insideUnitCircle * spawnRaduis;
        Vector3 spawnPosition = new Vector3(randomPoint.x, 0f, randomPoint.y);

        // generator random endmy
        int randomEnemy = Random.Range(0, 4);
        GameObject spawnEnemy;
        if (randomEnemy == 0)
            spawnEnemy = GetEnemyTypeOne();
        else if(randomEnemy == 1)
            spawnEnemy = GetEnemyTypeTwo();
        else if (randomEnemy == 2)
            spawnEnemy = GetEnemyTypeTwo();
        else
            spawnEnemy = GetEnemyTypeTwo();

        spawnEnemy.transform.position = spawnPosition;
        StartCoroutine(DelaySpawn());
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(spawnTime);
        isSpawnEnemy = false;

    }

    public void RestartGame()
    {
        // reset all enemies
        foreach (var e in enemy1)
        {
            e.SetActive(false);
            e.transform.position = Vector3.one * 10;
        }
        foreach (var e in enemy2)
        {
            e.SetActive(false);
            e.transform.position = Vector3.one * 10;
        }
        restartScreen.SetActive(false);
        player.gameObject.SetActive(true);
        PlayerControler.Instance.health = 100;
        level = 0;
        PlayerControler.Instance.Init();
        Time.timeScale = 1f;

    }

    public void GameOverScreen ()
    {
        restartScreen.SetActive(true);
        Time.timeScale = 0f;
    }


}
