using UnityEngine;

/// <summary>
/// A ScriptableObject that represents a damage modifier for bullets. It increases the bullet's damage by a specified amount.
/// </summary>
[CreateAssetMenu(fileName = "DamageModifier", menuName = "Decorators/DamageModifier")]
public class DamageModifier : ScriptableObject, IBulletModifier
{
    /// <summary>
    /// The amount of additional damage this modifier applies to the bullet.
    /// </summary>
    [SerializeField] private int additionalDamage;

    /// <summary>
    /// Returns the color associated with this damage modifier, typically used for UI representation or visual effects.
    /// </summary>
    public Color ModifierColor => Color.red;

    /// <summary>
    /// Applies the damage modification to the given bullet data, increasing its damage property.
    /// </summary>
    /// <param name="bulletData">The BulletDataSO instance to modify.</param>
    public void ModifyBulletData(BulletDataSO bulletData)
    {
        bulletData.damage += additionalDamage;
    }
}
