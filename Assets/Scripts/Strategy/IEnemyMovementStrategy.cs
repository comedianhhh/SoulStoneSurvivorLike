using UnityEngine;

/// <summary>
/// Defines an interface for enemy movement strategies.
/// This interface outlines a contract for implementing different
/// types of movement behaviors for enemies.
/// </summary>
public interface IEnemyMovementStrategy
{
    /// <summary>
    /// Moves the enemy based on a specific strategy.
    /// </summary>
    /// <param name="enemyTransform">The transform of the enemy game object. This provides the current position and orientation of the enemy.</param>
    /// <param name="target">The transform of the target that the enemy is interacting with, typically the player or a point of interest.</param>
    void Move(Transform enemyTransform, Transform target);
}
