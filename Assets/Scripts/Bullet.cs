using UnityEngine;

/// <summary>
/// Represents a projectile that can deal damage upon impact. Implements the IDoDamage interface to specify damage dealing behavior.
/// </summary>
public class Bullet : MonoBehaviour, IDoDamage
{
    /// <summary>
    /// A prefab representing the visual effect (e.g., a bullet hole) to instantiate upon bullet impact.
    /// </summary>
    [SerializeField] private GameObject bulletHole;

    /// <summary>
    /// The amount of damage this bullet applies to the target on impact.
    /// </summary>
    [SerializeField] private int damage = 35;

    /// <summary>
    /// Automatically destroys the bullet game object 3 seconds after it has been instantiated.
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, 3);
    }

    /// <summary>
    /// Sets the damage value of the bullet. Can be used to dynamically adjust bullet damage.
    /// </summary>
    /// <param name="damage">The new damage value for the bullet.</param>
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    /// <summary>
    /// Destroys the bullet game object upon collision with another object.
    /// </summary>
    private void OnCollisionEnter()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Retrieves the amount of damage this bullet will apply upon impact.
    /// </summary>
    /// <returns>The damage value of the bullet.</returns>
    public int GetDamage() => damage;

    /// <summary>
    /// Retrieves the prefab used for the impact visual effect (e.g., bullet hole).
    /// </summary>
    /// <returns>The prefab used for the impact effect.</returns>
    public GameObject GetHitPrefab() => bulletHole;
}
