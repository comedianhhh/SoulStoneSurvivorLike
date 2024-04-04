using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper class to facilitate the connection of the game object to the image used as helth bar
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// Exposes the property so the image (game object) can be easily attached to the inspector.
    /// </summary>
    [SerializeField] private Image bar;

    /// <summary>
    /// Update the image to reflect the object health bar.
    /// </summary>
    /// <param name="damage"></param>
    public void UpdateHealth(float damage)
    {
        bar.fillAmount = damage;
    }
}
