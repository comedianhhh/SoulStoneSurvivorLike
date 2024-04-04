using System.Reflection;
using UnityEngine;

public class GMPauseState : IState<GameManager>
{
    public void EnterState(GameManager gm)
    {
        //Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        Time.timeScale = 0;
    }

    public void ExitState(GameManager gm)
    {
        //Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        Time.timeScale = 1;
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
