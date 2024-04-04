using System.Reflection;
using UnityEngine;

public class GMMainMenuState : IState<GameManager>
{
    public void EnterState(GameManager gm)
    {
        //Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        if (gm.SessionManager.IsSessionActive) gm.SessionManager.EndMatch();
        AudioManager.Instance.PlayMusic("Theme");
        EventBus.Instance.Publish(new ShowCurrencyMangerUI());
        EventBus.Instance.Publish(new ShowGameSessionMangerUI());
    }

    public void ExitState(GameManager gm)
    {
        //Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        EventBus.Instance.Publish(new HideCurrencyMangerUI());
        EventBus.Instance.Publish(new HideGameSessionMangerUI());
    }

    public void HandleInput(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void ResumeState(GameManager gm)
    {
        //Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void Update(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }
}
