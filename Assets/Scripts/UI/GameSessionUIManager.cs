using UnityEngine;
using TMPro;
using static GameSessionManager; // Ensures access to GameSessionManager.NewSessionAddedEvent

/// <summary>
/// Manages the UI display of game session data, updating the UI to reflect current and new game sessions.
/// </summary>
public class GameSessionUIManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject sessionItemPrefab; // Prefab for displaying each session item.

    [SerializeField]
    private Transform sessionsContainer; // Parent object in the UI for session items.

    private GameSessionManager gameSessionManager; // Reference to the GameSessionManager.

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        gameSessionManager = GameManager.Instance.SessionManager;

        if (gameSessionManager != null)
        {
            DisplaySessions();
            EventBus.Instance.Subscribe<NewSessionAddedEvent>(OnNewSessionAdded);
        }
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<NewSessionAddedEvent>(OnNewSessionAdded);
        }
        UnsubscribeFromEvents();
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles the event when a new session is added, updating the UI accordingly.
    /// </summary>
    /// <param name="eventData">The data for the new session.</param>
    private void OnNewSessionAdded(object eventData)
    {
        var newSessionEvent = (NewSessionAddedEvent)eventData;
        AddSessionToUI(newSessionEvent.SessionData);
    }

    #endregion

    #region UI Update Methods

    /// <summary>
    /// Populates the UI with existing session data from the GameSessionManager.
    /// </summary>
    private void DisplaySessions()
    {
        var sessions = gameSessionManager.GetSessions();

        foreach (var session in sessions)
        {
            AddSessionToUI(session);
        }
    }

    /// <summary>
    /// Adds a single session's data to the UI.
    /// </summary>
    /// <param name="session">The session data to be displayed.</param>
    private void AddSessionToUI(MatchData session)
    {
        var item = Instantiate(sessionItemPrefab, sessionsContainer);
        item.GetComponentInChildren<TextMeshProUGUI>().text = FormatSessionData(session);
    }

    /// <summary>
    /// Formats session data into a string for display in the UI.
    /// </summary>
    /// <param name="session">The session data to format.</param>
    /// <returns>A string representing the formatted session data.</returns>
    private string FormatSessionData(MatchData session)
    {
        return $"Start: {session.StartTime}\nEnd: {session.EndTime}\nDuration: {session.Duration}";
    }

    #endregion

    /// <summary>
    /// Subscribes to necessary events on the event bus.
    /// </summary>
    private void SubscribeToEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Subscribe<ShowGameSessionMangerUI>(OnShowMenu);
            EventBus.Instance.Subscribe<HideGameSessionMangerUI>(OnHideMenu);
        }
    }

    /// <summary>
    /// Unsubscribes from events on the event bus.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<ShowGameSessionMangerUI>(OnShowMenu);
            EventBus.Instance.Unsubscribe<HideGameSessionMangerUI>(OnHideMenu);
        }
    }

    #region Menu Visibility Management

    /// <summary>
    /// Handles the event to show the Options Menu.
    /// </summary>
    /// <param name="eventData">The event data associated with showing the options menu. Currently unused but can be extended for future use.</param>
    private void OnShowMenu(object eventData)
    {
        // Code to show the Options Menu
        ToggleMenuVisibility(true);
    }

    /// <summary>
    /// Handles the event to hide the Options Menu.
    /// </summary>
    /// <param name="eventData">The event data associated with hiding the options menu. Currently unused but can be extended for future use.</param>
    private void OnHideMenu(object eventData)
    {
        // Code to hide the Options Menu
        ToggleMenuVisibility(false);
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
