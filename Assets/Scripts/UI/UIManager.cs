using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to create, show and update arrows in the UI
/// pointing to the enemies in the scene that are not visible on the screen.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Exposes the properties for easy access in the Inspector.
    /// </summary>
    [SerializeField] private RectTransform enemyIndicator;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Dictionary<GameObject, RectTransform> enemies = new Dictionary<GameObject, RectTransform>();
    [SerializeField] private GameObject player;

    /// <summary>
    /// Cache a reference to the Player, so it can access their position to set up the UI.
    /// List to the events from the Enemy Manager class.
    /// </summary>
    public void Start()
    {
        SubscribeToEvents();
        player = GameObject.Find("PlayerController");
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    /// <summary>
    /// Method created to listen to the OnDestroyEnemy event from the Event Manager class.
    /// Destroy the UI arrow and remove the key/value pair from the Dictionary
    /// </summary>
    /// <param name="sender">The object that invoked the event</param>
    /// <param name="e">An Enemy Object</param>
    private void EnemyManager_OnDestroyEnemy(object sender, EnemyManager.OnEnemyArgs e)
    {
        Destroy(enemies[e.enemy].gameObject);
        enemies.Remove(e.enemy);
    }

    /// <summary>
    /// Method created to listen to the OnAddEnemy event from the Event Manager class.
    /// Create a new arrow to rerepsent the new Enemy and add those references
    /// into a Dictionary.
    /// </summary>
    /// <param name="sender">The object that invoked the event</param>
    /// <param name="e">An Enemy Object</param>
    private void EnemyManager_OnAddEnemy(object sender, EnemyManager.OnEnemyArgs e)
    {
        RectTransform go = Instantiate(enemyIndicator, Vector2.zero, Quaternion.identity);
        go.transform.SetParent(transform, false);
        enemies.Add(e.enemy, go);
    }

    /// <summary>
    /// Update the position of the UI arrows.
    /// It is done at the LateUpdate() to make sure all the objects already moved.
    /// Finds the direction from the Player to each Enemy and position the arrow
    /// on the screen (converts the direction from 3D to 2D) if the Enemy is
    /// not visible on the screen.
    /// </summary>
    public void LateUpdate()
    {
        foreach(KeyValuePair<GameObject, RectTransform> item in  enemies)
        {
            Vector3 direction = (item.Key.transform.position - player.transform.position).normalized;
            float distance = Vector3.Distance(item.Key.transform.position, player.transform.position);
            item.Value.anchoredPosition = new Vector3(direction.x, direction.z, 0) * 500;  
            item.Value.eulerAngles = new Vector3(0, 0, GetAngleFromVector(direction));
            item.Value.gameObject.SetActive(distance > 12);
        }
    }

    /// <summary>
    /// Helper method to convert a direction (Vector3) into an euler angle
    /// </summary>
    /// <param name="dir">The direction (Vector3)</param>
    /// <returns>float: an euler angle</returns>
    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    /// <summary>
    /// Helper method to convert a direction (Vector3) into an euler angle
    /// </summary>
    /// <param name="dir">The direction (Vector3)</param>
    /// <returns>int: an euler angle</returns>
    public int GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }

    /// <summary>
    /// Subscribes to necessary events on the event bus.
    /// </summary>
    private void SubscribeToEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Subscribe<ShowOptionsMenuEvent>(OnShowMenu);
            EventBus.Instance.Subscribe<HideOptionsMenuEvent>(OnHideMenu);
        }
        enemyManager.OnAddEnemy += EnemyManager_OnAddEnemy;
        enemyManager.OnDestroyEnemy += EnemyManager_OnDestroyEnemy;
    }

    /// <summary>
    /// Unsubscribes from events on the event bus.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<ShowOptionsMenuEvent>(OnShowMenu);
            EventBus.Instance.Unsubscribe<HideOptionsMenuEvent>(OnHideMenu);
        }
        enemyManager.OnAddEnemy -= EnemyManager_OnAddEnemy;
        enemyManager.OnDestroyEnemy -= EnemyManager_OnDestroyEnemy;
    }

    #region Menu Visibility Management

    /// <summary>
    /// Handles the event to show the Options Menu.
    /// </summary>
    /// <param name="eventData">The event data associated with showing the options menu. Currently unused but can be extended for future use.</param>
    private void OnShowMenu(object eventData)
    {
        // Code to show the Options Menu
        ToggleMenuVisibility(false);
    }

    /// <summary>
    /// Handles the event to hide the Options Menu.
    /// </summary>
    /// <param name="eventData">The event data associated with hiding the options menu. Currently unused but can be extended for future use.</param>
    private void OnHideMenu(object eventData)
    {
        // Code to hide the Options Menu
        ToggleMenuVisibility(true);
    }

    /// <summary>
    /// Toggles the visibility of the options menu UI.
    /// </summary>
    /// <param name="isVisible">Whether the options menu should be visible.</param>
    public void ToggleMenuVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
    #endregion
}
