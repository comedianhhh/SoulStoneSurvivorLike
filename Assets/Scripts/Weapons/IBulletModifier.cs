using UnityEngine;

/// <summary>
/// Defines the functionality for a bullet modifier which can alter the properties of a bullet.
/// </summary>
public interface IBulletModifier
{
    /// <summary>
    /// Gets the color associated with the modifier. This color could be used for visual feedback or UI representation.
    /// </summary>
    Color ModifierColor { get; }

    /// <summary>
    /// Applies modifications to the specified bullet data, altering its properties such as damage, speed, or effects.
    /// </summary>
    /// <param name="bulletData">The bullet data to modify.</param>
    void ModifyBulletData(BulletDataSO bulletData);
}
