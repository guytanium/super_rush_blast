using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{

    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING}

    ;

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
        public bool waitForEnemiesToDie;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;
    private float playerSearchCountdown = 1f;
    private float playerDistanceCountdown = 0f;

    private SpawnState state = SpawnState.COUNTING;



    //players position
    private Transform playerTransform;
    //Holds Position of the current Furthest Spawn Location
    private Transform furthestSpawnPoint;
    //Find all Disrances from player
    private float distance = 0.0f;
    // An array of the spawn points this enemy can spawn from..
    // add randomness to spawn
    public float spawnPointRandomRadius = 0.5f;
    //Finds Furthest Distance from player
    private float furthestDistance = 0.0f;

    private Transform[] spawnPoints;
    private int spawnsCount;

    void Start()
    {
        //set up player association
        playerTransform = GameObject.FindWithTag("Player").transform;  

        waveCountdown = timeBetweenWaves;

        //set up the array of spawns (children of gameobject)
        spawnsCount = 0;
        foreach (Transform i in this.transform)
        {
            spawnsCount++;
        }
        spawnPoints = new Transform[spawnsCount];
        spawnsCount = 0;
        foreach (Transform i in this.transform)
        {
            spawnPoints[spawnsCount] = i;
            spawnsCount++;
        }
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn points in array");
        }

    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            //check there are no alive enemies

                if (!EnemyIsAlive())
                {
                    WaveCompleted();
                }


            else
            {
                return;
            }

        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

        //set up player association if they've disappeared only every second
        playerSearchCountdown -= Time.deltaTime;
        if (playerSearchCountdown <= 0f)
        {
            playerSearchCountdown = 1f;
            //find the player
            playerTransform = GameObject.FindWithTag("Player").transform;  

            if (playerTransform.transform == null)
            {
                Debug.LogError("No player, dead?");
                return;
            }
        }

        //just get the furthest spawn point
        playerDistanceCountdown -= Time.deltaTime;
        if (playerDistanceCountdown <= 0f)
        {
            playerSearchCountdown = 0.3f;

            //look at the array
            foreach (Transform point in spawnPoints)
            {
                // get the distance between each spawn and the player
                distance = Vector3.Distance(playerTransform.position, point.position);  
                //find the spawn furthest away from the player
                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    furthestSpawnPoint = point;
                }
            }
        }
    }

	

    void WaveCompleted()
    {

        //begin a new round
        Debug.Log("wave completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("all waves complete! looping...");
        }
        else
        {
            nextWave++;
        }
    }


    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;

    }

    bool WaitingForEnemies(Wave _wave)
    {
        if (_wave.waitForEnemiesToDie == true)
        {
            Debug.Log("NOT WAITING");
            return true;
        }
        return false;
    }


    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        if (_wave.waitForEnemiesToDie == false)
        {
            //state = SpawnState.WAITING;
            WaveCompleted();
            yield break;
        }
        else
        {
            state = SpawnState.WAITING;
        }

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {

        Instantiate(_enemy, furthestSpawnPoint.position + new Vector3(Random.insideUnitSphere.x * spawnPointRandomRadius, Random.insideUnitSphere.y * spawnPointRandomRadius, 0), furthestSpawnPoint.rotation); 
        Debug.Log("Spawning Enemy: " + _enemy.name);
        furthestDistance = float.MinValue;

    }
}
