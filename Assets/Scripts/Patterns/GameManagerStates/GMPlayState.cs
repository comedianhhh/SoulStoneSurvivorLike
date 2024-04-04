using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMPlayState : IState<GameManager>
{
    public void EnterState(GameManager gm)
    {
        //Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        SceneManager.LoadScene(1);
        gm.SessionManager.StartMatch();
    }

    public void ExitState(GameManager gm)
    {
        //Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        SceneManager.LoadScene(0);
        gm.SessionManager.EndMatch();
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
