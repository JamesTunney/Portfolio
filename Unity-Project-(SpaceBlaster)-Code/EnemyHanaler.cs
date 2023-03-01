using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHanaler : MonoBehaviour
{
    float time = 0;
    float enemyTimeTwo = 0;
    float asteroidTimer = 0;
    float iframeTime = 0;
    bool iframeOn = false;

    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject playerPrefab;
    public GameObject numberPrefab;
    public GameObject enemyShot;
    public GameObject bossPrefab;
    public GameObject asteroidPrefab;

    GameObject player;
    GameObject scoreText;
    GameObject healthText;
    GameObject bossHealthText;
    GameObject boss;

    static int score;

    int playerHealth = 5;

    int bossHealth = 100;

    float shootTimer = 0;
    Vector3 currentLocation;
    private List<GameObject> enemyBulletList = new List<GameObject>();

    private List<GameObject> enemyList = new List<GameObject>();
    private List<GameObject> enemyListTwo = new List<GameObject>();
    private List<GameObject> asteroidList = new List<GameObject>();

    private List<GameObject> playerBullets = new List<GameObject>();
    public GameObject playerShot;

    private bool isCurrentlyColliding = false;

    private bool spawnEnemyTwo = false;
    private bool bossFight = false;
    private bool spawnBoss = false;
    public bool startGame = true;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f);

        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        
        scoreText = (GameObject)Instantiate(numberPrefab, position: new Vector3(-3f, 6.36f, 0f), Quaternion.Euler(0, 0, 0));

        bossHealthText = (GameObject)Instantiate(numberPrefab, position: new Vector3(-3f, -5f, 0f), Quaternion.Euler(0, 0, 0));

        scoreText.GetComponentInChildren<TextMesh>().text = "Score: " + (score).ToString();

        if (startGame)
        {
            healthText = (GameObject)Instantiate(numberPrefab, position: new Vector3(-10f, 6.36f, 0f), Quaternion.Euler(0, 0, 0));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            SpriteRenderer m_SpriteRenderer = (player.transform.GetChild(0).gameObject).GetComponent<SpriteRenderer>();

            PlayerShoot();

            if (bossFight)
            {
                if (spawnBoss == false)
                {
                    Vector3 spawnPosition = new Vector3(10f, 0f, 0f);
                    boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
                    spawnBoss = true;
                    playerHealth = 10;
                }
                bossHealthText.GetComponentInChildren<TextMesh>().text = "Boss Health: " + (bossHealth).ToString();
                if (bossHealth >= 50)
                {
                    BossAttack(boss);
                }

                if (bossHealth < 50)
                {
                    time = time + Time.deltaTime;
                    enemyTimeTwo = enemyTimeTwo + Time.deltaTime;

                    if (enemyTimeTwo >= 5f)
                    {
                        spawnEnemyTwo = true;
                        enemyTimeTwo = 0;
                    }

                    BossAttack(boss);

                    if (time >= 1f)
                    {
                        float randomPosition = Random.Range(-4.3f, 4.3f);

                        Vector3 spawnPosition = new Vector3(12.5f, randomPosition, 0f);

                        GameObject spawnedCreature = Instantiate(firstEnemy, spawnPosition, Quaternion.identity);

                        enemyList.Add(spawnedCreature);

                        time = 0;
                    }
                }

            }
            else
            {
                if (spawnEnemyTwo)
                {
                    float randomPosition = Random.Range(-4.3f, 4.3f);

                    Vector3 spawnPosition = new Vector3(10f, 0f, 0f);

                    GameObject spawnedCreature = Instantiate(secondEnemy, spawnPosition, Quaternion.identity);

                    enemyListTwo.Add(spawnedCreature);

                    spawnEnemyTwo = false;
                }

                time = time + Time.deltaTime;
                enemyTimeTwo = enemyTimeTwo + Time.deltaTime;

                if (enemyTimeTwo >= 10f)
                {
                    spawnEnemyTwo = true;
                    enemyTimeTwo = 0;
                }

                if (time >= 1.5f)
                {
                    float randomPosition = Random.Range(-4.3f, 4.3f);

                    Vector3 spawnPosition = new Vector3(12.5f, randomPosition, 0f);

                    GameObject spawnedCreature = Instantiate(firstEnemy, spawnPosition, Quaternion.identity);

                    enemyList.Add(spawnedCreature);

                    time = 0;
                }
            }

            asteroidTimer = asteroidTimer + Time.deltaTime;

            if (asteroidTimer >= 15f)
            {
                asteroidTimer = 0;

                float randomPosition = Random.Range(-4.3f, 4.3f);

                Vector3 spawnPosition = new Vector3(randomPosition, 8.08f, 0f);

                GameObject spawnedCreature = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

                asteroidList.Add(spawnedCreature);
            }

            Cleanup();

            for (int i = 0; i < enemyListTwo.Count; i++)
            {
                ShootE(enemyListTwo[i]);
            }

            //Asteroid Collisions. 
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (iframeOn == false)
                {
                    if (CircleCollision(player, asteroidList[i]))
                    {
                        m_SpriteRenderer.color = Color.red;
                        playerHealth -= 1;
                    }
                }

            }
            for (int i = 0; i < asteroidList.Count; i++)
            {
                for (int x = 0; x < enemyBulletList.Count; x++)
                {
                    if (CircleCollision(enemyBulletList[x], asteroidList[i]))
                    {
                        Destroy(enemyBulletList[x]);
                        enemyBulletList.RemoveAt(i);
                        x = -1;
                    }

                }
            }

            for (int i = 0; i < asteroidList.Count; i++)
            {
                for (int x = 0; x < enemyList.Count; x++)
                {
                    if (CircleCollision(enemyList[x], asteroidList[i]))
                    {
                        Destroy(enemyList[x]);
                        enemyList.RemoveAt(x);
                        x = -1;
                    }
                }
            }

            for (int i = 0; i < asteroidList.Count; i++)
            {
                for (int x = 0; x < playerBullets.Count; x++)
                {
                    if (CircleCollision(playerBullets[x], asteroidList[i]))
                    {
                        Destroy(playerBullets[x]);
                        playerBullets.RemoveAt(x);
                        x = -1;
                    }
                }
            }



            for (int i = 0; i < enemyList.Count; i++)
            {
                if (iframeOn == false)
                {
                    if (CircleCollision(player, enemyList[i]))
                    {
                        m_SpriteRenderer.color = Color.red;
                        playerHealth -= 1;
                    }
                }

            }

            for (int i = 0; i < enemyListTwo.Count; i++)
            {
                if (iframeOn == false)
                {
                    if (CircleCollision(player, enemyListTwo[i]))
                    {
                        m_SpriteRenderer.color = Color.red;
                        playerHealth -= 1;
                    }
                }

            }

            for (int i = 0; i < enemyBulletList.Count; i++)
            {
                if (iframeOn == false)
                {
                    if (CircleCollision(player, enemyBulletList[i]))
                    {
                        m_SpriteRenderer.color = Color.red;
                        playerHealth -= 1;
                    }
                }

            }

            for (int x = 0; x < playerBullets.Count; x++)
            {

                for (int i = 0; i < enemyList.Count; i++)
                {

                    if (CircleCollision(playerBullets[x], enemyList[i]))
                    {
                        Destroy(playerBullets[x]);
                        Destroy(enemyList[i]);
                        playerBullets.RemoveAt(x);
                        enemyList.RemoveAt(i);
                        x = -1;
                        i = -1;
                        score += 100;
                    }
                }
            }

            if (bossFight)
            {
                for (int x = 0; x < playerBullets.Count; x++)
                {

                    if (CircleCollision(playerBullets[x], boss))
                    {
                        Destroy(playerBullets[x]);
                        playerBullets.RemoveAt(x);
                        x = -1;
                        bossHealth -= 1;

                    }
                }
            }

            for (int x = 0; x < playerBullets.Count; x++)
            {

                for (int i = 0; i < enemyListTwo.Count; i++)
                {

                    if (CircleCollision(playerBullets[x], enemyListTwo[i]))
                    {
                        Destroy(playerBullets[x]);
                        Destroy(enemyListTwo[i]);
                        playerBullets.RemoveAt(x);
                        enemyListTwo.RemoveAt(i);
                        x = -1;
                        i = -1;
                        score += 300;
                    }
                }
            }

            if (m_SpriteRenderer.color == Color.red)
            {
                iframeTime = iframeTime + Time.deltaTime;
                iframeOn = true;
            }
            if (iframeTime > 1.5f)
            {
                m_SpriteRenderer.color = Color.white;
                iframeOn = false;
                iframeTime = 0;
            }

            scoreText.GetComponentInChildren<TextMesh>().text = "Score: " + (score).ToString();

            healthText.GetComponentInChildren<TextMesh>().text = "Health: " + (playerHealth).ToString();

            if (playerHealth <= 0)
            {
                SceneManager.LoadScene("Game End");
            }
            if (bossHealth <= 0)
            {
                score = score + 10000;
                SceneManager.LoadScene("Game End");
            }

            if (score >= 10000f)
            {
                bossFight = true;
            }
        }
        
    }

    
    void Cleanup()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector3 currentLocation = enemyList[i].transform.position;
            if (currentLocation.x < -12f)
            {
                Destroy(enemyList[i]);
                enemyList.RemoveAt(i);
                i = 0;
            }
        }

        for (int i = 0; i < playerBullets.Count; i++)
        {
            Vector3 currentLocation = playerBullets[i].transform.position;
            if (currentLocation.x > 12f)
            {
                Destroy(playerBullets[i]);
                playerBullets.RemoveAt(i);
                i = 0;
            }
        }

        for (int i = 0; i < enemyBulletList.Count; i++)
        {
            Vector3 currentLocation = enemyBulletList[i].transform.position;
            if (currentLocation.x > 12f)
            {
                Destroy(enemyBulletList[i]);
                enemyBulletList.RemoveAt(i);
                i = 0;
            }
        }

    }

    private bool CircleCollision(GameObject objectA, GameObject objectB)
    {
        SpriteRenderer objectARender = (objectA.transform.GetChild(0).gameObject).GetComponent<SpriteRenderer>();
        SpriteRenderer objectBRender = (objectB.transform.GetChild(0).gameObject).GetComponent<SpriteRenderer>();

        Vector3 objectACenter = objectARender.bounds.center;
        Vector3 objectARadi = objectARender.bounds.extents;

        Vector3 objectBCenter = objectBRender.bounds.center;
        Vector3 objectBRadi = objectBRender.bounds.extents;

        float xDistance = objectACenter.x - objectBCenter.x;
        float yDistance = objectACenter.y - objectBCenter.y;

        float distance = (xDistance * xDistance) + (yDistance * yDistance);

        float radiaDistance = objectARadi.y + objectBRadi.y;

        if ((radiaDistance * radiaDistance) >= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShootE(GameObject shootingEnemy)
    {
        currentLocation = shootingEnemy.transform.position;

        shootTimer = shootTimer + Time.deltaTime;

        if(shootTimer >= 1)
        {
            GameObject currentObject;
            currentObject = (GameObject) Instantiate(enemyShot, position: new Vector3(currentLocation.x, currentLocation.y, currentLocation.z), Quaternion.Euler(0, 0, 0));
            enemyBulletList.Add(currentObject);
            shootTimer = 0;
        }
    }

    private void PlayerShoot()
    {
        if (Input.GetKeyDown("space"))
        {
            currentLocation = player.transform.position;
            Debug.Log("Fires");
            GameObject currentObject;
            currentObject = (GameObject) Instantiate(playerShot, position: new Vector3(currentLocation.x, currentLocation.y, currentLocation.z), Quaternion.Euler(0, 0, 0));
            playerBullets.Add(currentObject);
        }
    }

    private void BossAttack(GameObject shootingEnemy)
    {
        currentLocation = shootingEnemy.transform.position;

        shootTimer = shootTimer + Time.deltaTime;

        if (shootTimer >= 1)
        {
            GameObject currentObject;
            currentObject = (GameObject)Instantiate(enemyShot, position: new Vector3(currentLocation.x, currentLocation.y, currentLocation.z), Quaternion.Euler(0, 0, 0));
            enemyBulletList.Add(currentObject);
            currentObject = (GameObject)Instantiate(enemyShot, position: new Vector3(currentLocation.x, currentLocation.y+3f, currentLocation.z), Quaternion.Euler(0, 0, 0));
            enemyBulletList.Add(currentObject);
            currentObject = (GameObject)Instantiate(enemyShot, position: new Vector3(currentLocation.x, currentLocation.y-3f, currentLocation.z), Quaternion.Euler(0, 0, 0));
            enemyBulletList.Add(currentObject);
            shootTimer = 0;
        }
    }

  
}
