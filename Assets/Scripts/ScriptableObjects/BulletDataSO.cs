using UnityEngine;

/// <summary>
/// Defines the attributes and behavior of a bullet.
/// </summary>
[CreateAssetMenu(fileName = "New Bullet", menuName = "Asset/Bullet", order = 0)]
public class BulletDataSO : ScriptableObject
{
    /// <summary>
    /// The name of the bullet.
    /// </summary>
    public new string name;

    /// <summary>
    /// The damage dealt by the bullet.
    /// </summary>
    public int damage;

    /// <summary>
    /// The speed at which the bullet moves.
    /// </summary>
    public float speed;

    /// <summary>
    /// A prefab representing a special effect that occurs when the bullet hits a target.
    /// </summary>
    public GameObject specialEffectPrefab;

    /// <summary>
    /// A sprite representing the bullet in the game's UI or HUD.
    /// </summary>
    public Sprite bulletSprite;

    /// <summary>
    /// The color of the bullet. This can be used to dynamically change the appearance of the bullet.
    /// </summary>
    public Color bulletColor;

    /// <summary>
    /// The material used for rendering the bullet. This is dynamically created based on <see cref="bulletColor"/>.
    /// </summary>
    public Material material;

    /// <summary>
    /// Initializes the bullet's material and sets its color based on the defined <see cref="bulletColor"/>.
    /// </summary>
    private void OnEnable()
    {
        material = new Material(Shader.Find("Sprites/Default"));
        material.color = bulletColor;
    }
}
