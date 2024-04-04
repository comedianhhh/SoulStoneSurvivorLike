using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerWeapon;

/// <summary>
/// Controls the UI representation of bullet types for the player,
/// dynamically updating the display as bullets are added or removed.
/// </summary>
public class PlayerControllerUI : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // Prefab used to represent bullets in the UI.
    [SerializeField] private RectTransform panel; // Panel that holds the bullet representations.
    private List<GameObject> bullets = new List<GameObject>(); // List of bullet UI GameObjects.

    /// <summary>
    /// Ensures necessary references are set up on object initialization.
    /// </summary>
    private void Awake()
    {
        // Fallback to finding the RectTransform in children if not set.
        panel ??= GetComponentInChildren<RectTransform>();
    }

    /// <summary>
    /// Subscribes to bullet addition and removal events when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        EventBus.Instance.Subscribe<WeaponBulletAdded>(OnWeaponBulletAdded);
        EventBus.Instance.Subscribe<WeaponBulletRemoved>(OnWeaponBulletRemoved);
    }

    /// <summary>
    /// Unsubscribes from bullet addition and removal events when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<WeaponBulletAdded>(OnWeaponBulletAdded);
        EventBus.Instance.Unsubscribe<WeaponBulletRemoved>(OnWeaponBulletRemoved);
    }

    /// <summary>
    /// Handles the event of a bullet being added by instantiating its UI representation.
    /// </summary>
    /// <param name="eventData">Data associated with the bullet addition event.</param>
    private void OnWeaponBulletAdded(object eventData)
    {
        var data = (WeaponBulletAdded)eventData;
        BulletDataSO bulletData = data.BulletData;

        GameObject bulletUI = Instantiate(prefab, panel);
        ConfigureBulletUI(bulletUI, bulletData);
        bullets.Add(bulletUI);
        // Updates the color of all bullet representations.
        UpdateBulletColors(bulletData.bulletColor);
    }

    /// <summary>
    /// Handles the event of a bullet being removed by updating the UI accordingly.
    /// </summary>
    /// <param name="eventData">Data associated with the bullet removal event.</param>
    private void OnWeaponBulletRemoved(object eventData)
    {
        var data = (WeaponBulletRemoved)eventData;
        BulletDataSO bulletData = data.BulletData;

        // Remove the first bullet.
        if (bullets.Count > 0)
        {
            GameObject bulletUI = bullets[0];
            bullets.Remove(bulletUI);
            Destroy(bulletUI);
            // Reflects removal in the UI's color scheme.
            UpdateBulletColors(bulletData.bulletColor);
        }
    }

    /// <summary>
    /// Configures the visual appearance of a bullet UI element based on the provided BulletDataSO.
    /// </summary>
    /// <param name="bulletUI">The GameObject representing the bullet in the UI.</param>
    /// <param name="bulletData">The data object containing bullet properties.</param>
    private void ConfigureBulletUI(GameObject bulletUI, BulletDataSO bulletData)
    {
        Image image = bulletUI.GetComponent<Image>();
        image.color = bulletData.bulletColor;
        image.sprite = bulletData.bulletSprite;
    }

    /// <summary>
    /// Updates the color of all bullet representations in the UI to reflect the most recent bullet data color.
    /// </summary>
    /// <param name="color">The color to apply to all bullet UI elements.</param>
    private void UpdateBulletColors(Color color)
    {
        foreach (GameObject bulletUI in bullets)
        {
            Image image = bulletUI.GetComponent<Image>();
            image.color = color;
        }
    }
}
