using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the application of modifiers to a bullet's data, such as damage and speed, 
/// and dynamically updates its visual representation based on those modifiers.
/// </summary>
public class BulletDecoratorManager : MonoBehaviour
{
    [SerializeField] private BulletDataSO currentBulletData; // Holds the current state of bullet data.
    [SerializeField] private List<IBulletModifier> currentModifiers = new(); // List of active bullet modifiers.
    [SerializeField] private Sprite sprite; // Default sprite for the bullet.

    /// <summary>
    /// Initializes the bullet data with a new instance upon awakening.
    /// </summary>
    private void Awake()
    {
        currentBulletData = CreateNewBulletDataInstance();
    }

    /// <summary>
    /// Adds a modifier to the bullet and updates its properties accordingly.
    /// </summary>
    /// <param name="modifier">The modifier to add to the current bullet.</param>
    public void AddModifier(IBulletModifier modifier)
    {
        currentModifiers.Add(modifier);
        ApplyModifiers();
    }

    /// <summary>
    /// Removes a modifier from the bullet and updates its properties accordingly.
    /// </summary>
    /// <param name="modifier">The modifier to remove from the current bullet.</param>
    public void RemoveModifier(IBulletModifier modifier)
    {
        currentModifiers.Remove(modifier);
        ApplyModifiers();
    }

    /// <summary>
    /// Applies all current modifiers to the bullet data and updates the bullet's visual representation based on its modified properties.
    /// </summary>
    private void ApplyModifiers()
    {
        currentBulletData = CreateNewBulletDataInstance();

        foreach (var modifier in currentModifiers)
        {
            modifier.ModifyBulletData(currentBulletData);
        }

        // Logic to blend color based on damage and speed intensities
        float damageIntensity = Mathf.InverseLerp(10, 50, currentBulletData.damage);
        float speedIntensity = Mathf.InverseLerp(10f, 30f, currentBulletData.speed);
        Color damageColor = Color.Lerp(Color.white, Color.red, damageIntensity);
        Color speedColor = Color.Lerp(Color.white, Color.blue, speedIntensity);
        currentBulletData.material.color = Color.Lerp(damageColor, speedColor, 0.5f);
        currentBulletData.bulletColor = Color.Lerp(damageColor, speedColor, 0.5f);
    }

    /// <summary>
    /// Provides access to the current bullet data.
    /// </summary>
    public BulletDataSO BulletDataSO => currentBulletData;

    /// <summary>
    /// Creates a new instance of BulletDataSO with default values.
    /// </summary>
    /// <returns>A new instance of BulletDataSO initialized with default values.</returns>
    public BulletDataSO CreateNewBulletDataInstance()
    {
        BulletDataSO newInstance = ScriptableObject.CreateInstance<BulletDataSO>();
        newInstance.damage = 10;
        newInstance.speed = 10f;
        newInstance.bulletSprite = sprite;
        newInstance.bulletColor = Color.white;
        newInstance.material.color = Color.white;
        return newInstance;
    }
}
