using UnityEngine;

/// <summary>
/// A ScriptableObject that represents a speed modifier for bullets. It increases the bullet's speed by a specified amount.
/// </summary>
[CreateAssetMenu(fileName = "SpeedModifier", menuName = "Decorators/SpeedModifier")]
public class SpeedModifier : ScriptableObject, IBulletModifier
{
    /// <summary>
    /// The amount of additional speed this modifier applies to the bullet.
    /// </summary>
    [SerializeField] private float additionalSpeed;

    /// <summary>
    /// Returns the color associated with this speed modifier, typically used for UI representation or visual effects.
    /// </summary>
    public Color ModifierColor => Color.blue;

    /// <summary>
    /// Applies the speed modification to the given bullet data, increasing its speed property.
    /// </summary>
    /// <param name="bulletData">The BulletDataSO instance to be modified.</param>
    public void ModifyBulletData(BulletDataSO bulletData)
    {
        bulletData.speed += additionalSpeed;
    }
}
