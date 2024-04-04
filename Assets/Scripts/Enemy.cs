using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Expose properties that allow customization of this game object
    /// It also expose propertis for easy access through the Inspector
    /// </summary>
    [SerializeField, Range(1, 5)] private float moveSpeed = 1f;
//    [SerializeField, Range(7, 10)]private float turnSpeed = 10f;
    [SerializeField, Range(25, 100)] private float maxHealth = 100f;
    [SerializeField, Range(1, 5)] private float maxSleep = 5f;
    [SerializeField] private float numHealth = 0f;
    [SerializeField] private float numSleep = 0f;
    [SerializeField] private Transform target;
    [SerializeField] private bool isSleeping = true;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private List<AudioClip> clip = new List<AudioClip>();
    [SerializeField] private Transform drop;
    private AudioSource audioSouce;
    private GameObject enemies;
    private EnemyManager enemiesManager;
    [SerializeField]
    private IEnemyMovementStrategy movementStrategy;
    private List<IEnemyMovementStrategy> availableStrategies;

    /// <summary>
    /// Cache a reference for the EnemyManager game object using the GambeObject.Find()
    /// It is a costely function and should not be used in a Update() method, for example.
    /// The Enemy Manager maintain a list of enemys; however, the enemy should not have
    /// direct access to the Enemy Manager. An Event Broker pattern should be used in this case.
    /// </summary>
    private void Awake()
    {
        numHealth = maxHealth;
        enemies = GameObject.Find("EnemyManager");
        enemiesManager = enemies.GetComponent<EnemyManager>();
        audioSouce = GetComponent<AudioSource>();
        ChoseStrategy();
    }

    /// <summary>
    /// Chooses a movement strategy for the enemy at random from a list of available strategies.
    /// This method initializes the list of possible strategies and randomly selects one to be used.
    /// </summary>
    private void ChoseStrategy()
    {
        // Initialize strategies
        availableStrategies = new List<IEnemyMovementStrategy>()
        {
            new DirectChaseStrategy(),
            new RandomWanderStrategy()
            // Add other strategies here
        };
        // Randomly select a strategy from the list of available strategies
        movementStrategy = availableStrategies[Random.Range(0, availableStrategies.Count)];
    }

    /// <summary>
    /// Sets the enemy's movement strategy to the specified strategy.
    /// This allows for dynamic changes in behavior during gameplay.
    /// </summary>
    /// <param name="newStrategy">The new movement strategy to be set for the enemy.</param>
    public void SetMovementStrategy(IEnemyMovementStrategy newStrategy)
    {
        movementStrategy = newStrategy;
    }

    /// <summary>
    /// Cache a reference for the Player game object using the GambeObject.Find()
    /// It is a costely function and should not be used in a Update() method, for example.
    /// Also randomize the Enemy the move speed and the idle time.
    /// </summary>
    private void Start()
    {
        target = GameObject.Find("PlayerController").transform;
        moveSpeed = Random.Range(2, 5);
        maxSleep = Random.Range(1, maxSleep);
    }

    /// <summary>
    /// The first part controls the initial delay. It waits for sometime before start chasing the player.
    /// Every frame, check the direction to the Player and move towards them.
    /// </summary>
    private void Update()
    {
        if(isSleeping) {
            numSleep += Time.deltaTime;
            if(numSleep > maxSleep)
            {
                numSleep = 0;
                isSleeping = false;
            }
        } else
        {
            movementStrategy.Move(transform, target);
            /*
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += moveSpeed * Time.deltaTime * direction;
            transform.forward = Vector3.Slerp(transform.forward, direction, turnSpeed * Time.deltaTime);
            */
        }
    }

    /// <summary>
    /// Handles the collision. Because the object is set to be kinematic, physics does not controll its movement.
    /// If the collision happens with an object that implements the IDoDamage interface, access the damage amount
    /// and any prefab that can be used for special effects.
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<IDoDamage>(out IDoDamage bullet))
        {
            audioSouce.clip = clip[Random.Range(0, clip.Count)];
            audioSouce.Play();
            ContactPoint c = collision.GetContact(0);
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.back, c.normal);
            GameObject hole = Instantiate(bullet.GetHitPrefab(), c.point + (c.normal * -0.1f), hitRotation);
            hole.transform.SetParent(transform);
            TakeDamage(bullet.GetDamage());
        }
    }

    /// <summary>
    /// Method used to apply the damage received to the object health.
    /// It destroy itself if the health goes below 0, and update the healthbar otherwise.
    /// It should be replaced by a proper HealthSystem.
    /// </summary>
    /// <param name="damage"></param>
    private void TakeDamage(float damage)
    {
        numHealth -= damage;
        float health = numHealth / maxHealth;
        healthBar.UpdateHealth(health);
        if (numHealth <= 0) {
            Die();
            Destroy(gameObject); 
        } else if (health <= 0.50)
        {
            SetMovementStrategy(new DirectChaseStrategy());
        }
    }

    private void Die()
    {
        Instantiate(drop, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
    }

    /// <summary>
    /// Access the Enemy Manager and use the Remove() method to remove itself
    /// from its list of enemies. That is not the proper approach and it should
    /// be replaced by events. So the enemy run an event anouncing that it being destroyed
    /// and the listeners can react to it.
    /// </summary>
    private void OnDestroy()
    {
        enemiesManager?.Remove(gameObject);
    }

    /// <summary>
    /// Draw a line showing the enemy forward vector
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 target = transform.forward * .5f + transform.position + Vector3.up * 1;
        Gizmos.DrawLine(transform.position + Vector3.up * 1, target);
    }
}
