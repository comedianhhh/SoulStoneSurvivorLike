using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Manages the firing mechanics of the player's weapon, including handling different bullet types,
/// applying bullet modifiers, and managing bullet-related audio effects.
/// </summary>
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Rigidbody bullet; // Prefab of the bullet used for physical instantiation.
    [SerializeField] private AssetReference newBullet; // Reference to the new bullet asset for dynamic loading.
    [SerializeField] private List<AudioSO> audioClipList; // List of audio sound effects for bullet actions.
    [SerializeField] private BulletDataSO currentBulletData; // Currently selected bullet data.
    [SerializeField] private List<BulletDataSO> bullets; // Available bullet types.
    [SerializeField] private BulletDecoratorManager bulletDecoratorManager; // Manager for applying bullet modifiers.

    private AudioSource audioSource; // Audio source component for playing bullet sound effects.

    // Properties for bullet behavior.
    public float bulletSpeed = 20f;
    public int bulletDamage = 25;
    public float shootInterval = 0.5f;
    private float lastShootTime; // Timestamp of the last bullet shot.

    /// <summary>
    /// Initializes the weapon system, ensuring an audio source is available and setting up the bullet decorator manager.
    /// </summary>
    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        bulletDecoratorManager ??= GetComponent<BulletDecoratorManager>();
    }

    /// <summary>
    /// Prepares the initial bullet setup and subscribes to relevant events at the start.
    /// </summary>
    private void Start()
    {
        LoadAllBullets();
        currentBulletData = bulletDecoratorManager.BulletDataSO; // Sets the initial bullet data based on decorator manager's state.
        EventBus.Instance.Publish(new WeaponBulletAdded(currentBulletData)); // Notifies the system of the selected bullet.
    }

    /// <summary>
    /// Listens for player input to switch bullet modifiers or fire bullets, and applies modifiers as needed.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var damageModifier = Resources.Load<DamageModifier>("BulletModifiers/DamageModifier");
            if (damageModifier != null)
            {
                bulletDecoratorManager.AddModifier(damageModifier);
                currentBulletData = bulletDecoratorManager.BulletDataSO;
                EventBus.Instance.Publish(new WeaponBulletAdded(currentBulletData));
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var speedModifier = Resources.Load<SpeedModifier>("BulletModifiers/SpeedModifier");
            if (speedModifier != null)
            {
                bulletDecoratorManager.AddModifier(speedModifier);
                currentBulletData = bulletDecoratorManager.BulletDataSO;
                EventBus.Instance.Publish(new WeaponBulletAdded(currentBulletData));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var damageModifier = Resources.Load<DamageModifier>("BulletModifiers/DamageModifier");
            if (damageModifier != null)
            {
                bulletDecoratorManager.RemoveModifier(damageModifier);
                currentBulletData = bulletDecoratorManager.BulletDataSO;
                EventBus.Instance.Publish(new WeaponBulletRemoved(currentBulletData));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            var speedModifier = Resources.Load<SpeedModifier>("BulletModifiers/SpeedModifier");
            if (speedModifier != null)
            {
                bulletDecoratorManager.RemoveModifier(speedModifier);
                currentBulletData = bulletDecoratorManager.BulletDataSO;
                EventBus.Instance.Publish(new WeaponBulletRemoved(currentBulletData));
            }
        }
    }

    /// <summary>
    /// Dynamically loads all available bullet types from the Resources/Bullets directory.
    /// </summary>
    private void LoadAllBullets()
    {
        bullets = Resources.LoadAll<BulletDataSO>("Bullets").ToList();
    }

    /// <summary>
    /// Updates the current bullet data and publishes an event to notify other components of the change.
    /// </summary>
    /// <param name="bulletData">The new bullet data to be set as current.</param>
    public void SetBullet(BulletDataSO bulletData)
    {
        currentBulletData = bulletData;
        EventBus.Instance.Publish(new WeaponBulletChanged(bulletData));
    }

    /// <summary>
    /// Structs for event data.
    /// </summary>
    public struct WeaponBulletChanged
    {
        public BulletDataSO BulletData { get; private set; }

        public WeaponBulletChanged(BulletDataSO bulletData)
        {
            BulletData = bulletData;
        }
    }

    public struct WeaponBulletAdded
    {
        public BulletDataSO BulletData { get; private set; }

        public WeaponBulletAdded(BulletDataSO bulletData)
        {
            BulletData = bulletData;
        }
    }

    public struct WeaponBulletRemoved
    {
        public BulletDataSO BulletData { get; private set; }

        public WeaponBulletRemoved(BulletDataSO bulletData)
        {
            BulletData = bulletData;
        }
    }

    /// <summary>
    /// Checks if the shooting interval has elapsed before firing a bullet to prevent spamming.
    /// </summary>
    /// <param name="shootDirection">The direction in which the bullet is fired.</param>
    public void TryShoot(Vector3 shootDirection)
    {
        if (Time.time - lastShootTime >= shootInterval)
        {
            Shoot(shootDirection);
            lastShootTime = Time.time;
        }
    }

    /// <summary>
    /// Instantiates a bullet using the current bullet data and applies its properties and modifiers.
    /// </summary>
    /// <param name="shootDirection">The direction in which the bullet is fired.</param>
    public void Shoot(Vector3 shootDirection)
    {
        float bulletHeight = Random.Range(0.5f, 1.8f);
        Vector3 bulletPosition = transform.position + shootDirection + (Vector3.up * bulletHeight);

        AssetManager.Instance.Inst(newBullet, bulletPosition, Quaternion.identity,
            (instGO) =>
            {
                if (instGO != null)
                {
                    instGO.GetComponent<Bullet>().SetDamage(currentBulletData.damage);
                    instGO.GetComponent<TrailRenderer>().material = currentBulletData.material;
                    Rigidbody rb = instGO.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = shootDirection * currentBulletData.speed;
                    }
                    HandleWeaponSound();
                }
            });
    }

    /// <summary>
    /// Plays an audio effect from the list of available sounds when a bullet is fired.
    /// </summary>
    private void HandleWeaponSound()
    {
        AudioSO clip = audioClipList[Random.Range(0, audioClipList.Count)];
        AudioManager.Instance.PlaySound(clip);
    }
}
