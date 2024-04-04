using UnityEngine;

/// <summary>
/// Implements a direct chase movement strategy for an enemy.
/// This strategy makes the enemy directly move towards the target with a specified speed.
/// </summary>
public class DirectChaseStrategy : IEnemyMovementStrategy
{
    /// The speed at which the enemy moves towards the target.
    public float Speed { get; set; } = 3f;
    /// The speed at which the enemy turns to face the target. Helps in smooth rotation towards the target.
    public float TurnSpeed { get; set; } = 10f; 

    /// <summary>
    /// Moves the enemy directly towards the target position.
    /// </summary>
    /// <param name="enemyTransform">The transform of the enemy that will be moved towards the target.</param>
    /// <param name="target">The transform of the target that the enemy is moving towards.</param>
    public void Move(Transform enemyTransform, Transform target)
    {
        // Calculate the direction from the enemy to the target
        Vector3 direction = (target.position - enemyTransform.position).normalized;
        // Move the enemy towards the target
        enemyTransform.position += direction * Speed * Time.deltaTime;
        // Smoothly rotate the enemy to face the target direction
        enemyTransform.forward = Vector3.Slerp(enemyTransform.forward, direction, TurnSpeed * Time.deltaTime);
    }
}
