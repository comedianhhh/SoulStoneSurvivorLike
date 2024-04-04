using System.Reflection;
using UnityEngine;

public class GMOptionsMenuState : IState<GameManager>
{
    public void EnterState(GameManager gm)
    {
        EventBus.Instance.Publish(new ShowOptionsMenuEvent());
        Time.timeScale = 0;
    }

    public void ExitState(GameManager gm)
    {
        Time.timeScale = 1;
        EventBus.Instance.Publish(new HideOptionsMenuEvent());
    }

    public void HandleInput(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void ResumeState(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void Update(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }
}
