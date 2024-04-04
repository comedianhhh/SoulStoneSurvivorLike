using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class created to maintain a list of all enemies in scene.
/// Have two events, one executed when a new enemy is added and one
/// when an enemy is removed. It works as an intermediary and grant
/// "access" or information about the enemies in scene
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// OnAddEnemy: executed when a new enemy is added to the list
    /// OnDestroyEnemy: executed when an enemy is destroyed
    /// OnEmeyArgs: a customized class that extend the EventArgs, so it is possible
    /// to pass a the Enemy as parameter to the events
    /// </summary>
    public event EventHandler<OnEnemyArgs> OnAddEnemy;
    public event EventHandler<OnEnemyArgs> OnDestroyEnemy;
    public class OnEnemyArgs : EventArgs {
        public GameObject enemy;
    }
        
    /// <summary>
    /// The list of enemies, it does not need to be exposed in the inspector
    /// but is good to give a quick access to the enemies (for debug purpouse)
    /// </summary>
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private bool isSpawningFinished = false;
    private int spawnerFound = 0;

    private void Awake()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    /// <summary>
    /// Add the enemy into the list and execute the OnAddEnemy event
    /// </summary>
    /// <param name="enemy">The new enemy created</param>
    public void Add(GameObject enemy)
    {
        OnAddEnemy?.Invoke(this, new OnEnemyArgs { enemy = enemy });
        enemies.Add(enemy);
    }

    /// <summary>
    /// Remove an enemy from the list and execute the OnDestroyEnemy event
    /// </summary>
    /// <param name="enemy">The enemy that was destroyed</param>
    public void Remove(GameObject enemy)
    {
        enemies.Remove(enemy);
        OnDestroyEnemy?.Invoke(this, new OnEnemyArgs { enemy = enemy });
        CheckForStateChange();
    }

    /// <summary>
    /// Expose access to the List of enemies
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetEnemies()
    {
        return enemies; 
    }

    private void SubscribeToEvents()
    {
        EventBus.Instance.Subscribe<SpawningStarted>(OnSpawningStarted);
        EventBus.Instance.Subscribe<SpawningStopped>(OnSpawningStopped);
    }

    private void UnsubscribeToEvents()
    {
        EventBus.Instance.Unsubscribe<SpawningStarted>(OnSpawningStarted);
        EventBus.Instance.Unsubscribe<SpawningStopped>(OnSpawningStopped);
    }

    private void OnSpawningStarted(object obj)
    {
        isSpawningFinished = false;
        spawnerFound++;
    }

    private void OnSpawningStopped(object obj)
    {
        spawnerFound--;
        if (spawnerFound == 0)
        {
            isSpawningFinished = true;
        }
    }

    private void CheckForStateChange()
    {
        if (isSpawningFinished && enemies.Count == 0)
        {
            GameManager.Instance.ChangeState(new GMMainMenuState());
        }
    }
}
