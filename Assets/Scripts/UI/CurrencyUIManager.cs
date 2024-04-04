using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Player;

/// <summary>
/// Manages the UI representation of player currencies, specifically Soulstones.
/// </summary>
public class CurrencyUIManager : MonoBehaviour
{
    #region Fields and Properties
    public Player player;
    public GameObject currencyUIPrefab;
    public Transform contentPanel;
    [SerializeField] private Button btnUndo;
    private CommandInvoker commandInvoker;

    // Dictionary to map currency data to their UI elements.
    private Dictionary<StonesDataSO, GameObject> currencyUIMap = new Dictionary<StonesDataSO, GameObject>();
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        commandInvoker = new CommandInvoker();
        btnUndo.onClick.AddListener(() =>
        {
            commandInvoker.Undo();
            AudioManager.Instance.PlaySound("BtnClick");
        });
        EnsureContentPanelIsSet();
    }

    private void Start()
    {
        InitializeUI();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<SoulstoneUpdatedEvent>(OnSoulstoneUpdated);
        }
        UnsubscribeFromEvents();
    }

    #endregion

    #region Initialization
    /// <summary>
    /// Ensures necessary references are set, specifically the content panel.
    /// </summary>
    private void EnsureContentPanelIsSet()
    {
        if (contentPanel == null)
        {
            contentPanel = this.transform;
        }
    }

    /// <summary>
    /// Initializes the UI elements for each currency. Subscribes to currency update events.
    /// </summary>
    private void InitializeUI()
    {
        FindPlayerReference();
        InitializeCurrencyUIElements();
        SubscribeToCurrencyUpdates();
    }

    private void FindPlayerReference()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
    }

    private void InitializeCurrencyUIElements()
    {
        foreach (var soulstoneCache in player.GetStones())
        {
            if (soulstoneCache.soulstoneData != null)
            {
                currencyUIMap[soulstoneCache.soulstoneData] = GetCurrencyUIElement(soulstoneCache);
            }
        }
    }

    private void SubscribeToCurrencyUpdates()
    {
        if(EventBus.Instance != null)
        {
            EventBus.Instance.Subscribe<SoulstoneUpdatedEvent>(OnSoulstoneUpdated);
        }
    }

    private void OnSoulstoneUpdated(object eventData)
    {
        var soulstoneUpdatedEvent = (SoulstoneUpdatedEvent)eventData;
        UpdateCurrencyUI(soulstoneUpdatedEvent.UpdatedStones);
    }
    #endregion

    #region UI Updates
    /// <summary>
    /// Updates the UI element for a specific currency based on the provided currency data.
    /// </summary>
    /// <param name="updatedStone">The currency data that was updated.</param>
    private void UpdateCurrencyUI(SoulstoneCache updatedStone)
    {
        if (currencyUIMap.TryGetValue(updatedStone.soulstoneData, out GameObject currencyUI))
        {
            UpdateCurrencyElementUI(currencyUI, updatedStone);
        }
    }

    private void UpdateCurrencyElementUI(GameObject currencyUI, SoulstoneCache updatedStone)
    {
        TextMeshProUGUI currencyText = currencyUI.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = currencyUI.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        currencyText.text = updatedStone.soulstoneData.stoneName;
        amountText.text = updatedStone.quantity.ToString();
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Creates or retrieves the UI element (GameObject) associated with the provided currency data.
    /// If the UI element doesn't exist, it creates one and sets up its properties and event listeners.
    /// </summary>
    /// <param name="stone">The currency data for which to retrieve or create the UI element.</param>
    /// <returns>The UI element (GameObject) associated with the provided currency data.</returns>
    private GameObject GetCurrencyUIElement(SoulstoneCache stone)
    {
        if (currencyUIMap.ContainsKey(stone.soulstoneData))
            return currencyUIMap[stone.soulstoneData];
        GameObject currencyUI = Instantiate(currencyUIPrefab, contentPanel);
        TextMeshProUGUI currencyText = currencyUI.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountText = currencyUI.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        Image currencyImage = currencyUI.transform.Find("Icon").GetComponent<Image>();
        Button addButton = currencyUI.transform.Find("Add").GetComponent<Button>();
        Button removeButton = currencyUI.transform.Find("Remove").GetComponent<Button>();
        currencyText.text = stone.soulstoneData.stoneName;
        amountText.text = stone.quantity.ToString();
        currencyImage.sprite = stone.soulstoneData.stoneSprite;
        // Add 10 as an example
        addButton.onClick.AddListener(() => {
            /*
            player.UpdateCurrencyAmount(stone.soulstoneData, 10);
            */
            var addCommand = new AddSoulstoneCommand(player, stone.soulstoneData, 10);
            commandInvoker.ExecuteCommand(addCommand);
            AudioManager.Instance.PlaySound("BtnClick");
        });
        // Remove 10
        removeButton.onClick.AddListener(() => {
            /*
            player.UpdateCurrencyAmount(stone.soulstoneData, -10);
            */
            var removeCommand = new RemoveSoulstoneCommand(player, stone.soulstoneData, 10);
            commandInvoker.ExecuteCommand(removeCommand);
            AudioManager.Instance.PlaySound("BtnClick");
        }); 
        return currencyUI;
    }

    #endregion

    /// <summary>
    /// Subscribes to necessary events on the event bus.
    /// </summary>
    private void SubscribeToEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Subscribe<ShowCurrencyMangerUI>(OnShowMenu);
            EventBus.Instance.Subscribe<HideCurrencyMangerUI>(OnHideMenu);
        }
    }

    /// <summary>
    /// Unsubscribes from events on the event bus.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<ShowCurrencyMangerUI>(OnShowMenu);
            EventBus.Instance.Unsubscribe<HideCurrencyMangerUI>(OnHideMenu);
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
