using UnityEngine;

/// <summary>
/// Defines a wandering movement strategy for an enemy, 
/// allowing it to randomly select points near the player to move towards.
/// </summary>
public class RandomWanderStrategy : IEnemyMovementStrategy
{
    /// The speed at which the enemy moves towards the target.
    public float Speed { get; set; } = 2f;
    /// The speed at which the enemy turns to face the target. Helps in smooth rotation towards the target.
    public float TurnSpeed { get; set; } = 10f;
    private Vector3 targetPosition;
    private float minDistanceToTarget = 1f; // Minimum distance to consider as "reached" the target
    private float maxDistanceFromPlayer = 5f; // Max distance for the random point from the player

    /// <summary>
    /// Initializes a new instance of the RandomWanderStrategy class.
    /// </summary>
    public RandomWanderStrategy()
    {
        targetPosition = Vector3.zero;
    }

    /// <summary>
    /// Moves the enemy towards a randomly selected point near the player.
    /// If the current target position is reached, a new target is generated.
    /// </summary>
    /// <param name="enemyTransform">The transform of the enemy.</param>
    /// <param name="playerTransform">The transform of the player.</param>
    public void Move(Transform enemyTransform, Transform playerTransform)
    {
        if (targetPosition == Vector3.zero || Vector3.Distance(enemyTransform.position, targetPosition) < minDistanceToTarget)
        {
            // Generate a new target position around the player
            Vector3 randomDirection = Random.insideUnitSphere * maxDistanceFromPlayer;
            randomDirection += playerTransform.position;
            randomDirection.y = enemyTransform.position.y; // Keep the enemy on the same ground level
            targetPosition = randomDirection;
        }

        // Move towards the target position
        Vector3 direction = (targetPosition - enemyTransform.position).normalized;
        enemyTransform.position += direction * Speed * Time.deltaTime;
        enemyTransform.forward = Vector3.Slerp(enemyTransform.forward, direction, TurnSpeed * Time.deltaTime);
    }
}
