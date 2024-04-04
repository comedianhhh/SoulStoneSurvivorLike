using UnityEngine;

/// <summary>
/// Class created to control the enemy spawn. It can be configured
/// to spawn a number of enemies in a certan period of time.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Expose propertis to set up the spawner
    /// </summary>
    [SerializeField] private GameObject prefab;
    [SerializeField, Range(1, 10)] private int maxSpawn = 10;
    [SerializeField] private int numSpawn = 0;
    [SerializeField, Range(1, 5)] private float spawnTimeMax = 3f;
    [SerializeField] private bool isSpawning = true;
    private float spawnTime = 0f;
    private GameObject enemies;
    private EnemyManager enemiesManager;
    private Transform visual;

    /// <summary>
    /// Cache a refernece to the Enemy Manager and for the spawner visual component.
    /// </summary>
    private void Awake()
    {
        enemies = GameObject.Find("EnemyManager");
        enemiesManager = enemies.GetComponent<EnemyManager>();
        visual = transform.GetChild(0);
    }

    private void Start()
    {
        EventBus.Instance.Publish(new SpawningStarted());
    }

    /// <summary>
    /// All code was moved to a proper method Spawn()
    /// </summary>
    public void Update()
    {
        Spawn();
    }

    /// <summary>
    /// Public method to verify if the spawner is active or not. Can be used
    /// to check if it is time to summon a different type of enemy, or to 
    /// spawn a boss etc.
    /// </summary>
    /// <returns>bool: true if the spawner is active</returns>
    public bool IsSpawning ()
    {
        return isSpawning;
    }

    /// <summary>
    /// This method has a timer, and spawn a new enemy after a givin amount of time.
    /// Reset the timer after spawning a new enemy.
    /// Find a random position withing the spawner to spawn a new enemy.
    /// Add that enemy into the Enemy Manager. This direct access to the Enemy Manager
    /// is an easy way to do that, but it should be replaced by an event.
    /// </summary>
    public void Spawn()
    {
        spawnTime += Time.deltaTime;
        if (isSpawning && spawnTime > spawnTimeMax)
        {
            spawnTime = 0;
            numSpawn++;
            if (numSpawn == maxSpawn)
            {
                isSpawning = false;
                EventBus.Instance.Publish(new SpawningStopped());
            }
            float posx = Random.Range(-visual.localScale.x/2, visual.localScale.x/2);
            float posz = Random.Range(-visual.localScale.z/2, visual.localScale.z/2);
            Vector3 spawnPosition = transform.position + new Vector3(posx, -transform.position.y, posz);
            GameObject go = Instantiate(prefab, spawnPosition, Quaternion.identity);
            go.transform.SetParent(enemies.transform, false);
            enemiesManager.Add(go); 
        }
    }
}
