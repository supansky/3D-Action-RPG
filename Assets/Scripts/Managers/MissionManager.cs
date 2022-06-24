using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    public int CurLevel { get; private set; }
    public int MaxLevel { get; private set; }

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Mission manager starting...");

        network = service;

        UpdateData(0, 2);

        Status = ManagerStatus.Started;
    }

    public void GoToNext()
    {
        if (CurLevel < MaxLevel)
        {
            CurLevel++;
            string name = $"Level{CurLevel}";
            Debug.Log($"Loading {name}");
            SceneManager.LoadScene(name);
        }
        else
        {
            Debug.Log("Last level");
            Messenger.Broadcast(GameEvent.GAME_COMPLETE);
        }
    }
    
    public void ReachObjective()
    {
        //logic to handle multiple objectives
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }
    public void RestartCurrent()
    {
        string name = $"Level{CurLevel}";
        Debug.Log($"Loading {name}");
        SceneManager.LoadScene(name);
    }

    public void UpdateData(int curLevel, int maxLevel)
    {
        CurLevel = curLevel;
        MaxLevel = maxLevel;
    }

}
