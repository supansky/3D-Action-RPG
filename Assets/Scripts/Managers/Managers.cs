using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (InventoryManager))]
[RequireComponent(typeof (PlayerManager))]


public class Managers : MonoBehaviour
{
    public static InventoryManager Inventory { get; private set; }
    public static PlayerManager Player { get; private set; }

    private List<IGameManager> startSequence;

    private void Awake()
    {
        Inventory = GetComponent<InventoryManager>();
        Player = GetComponent<PlayerManager>();
        startSequence = new List<IGameManager>();
        startSequence.Add(Inventory);
        startSequence.Add(Player);

        StartCoroutine(StartupManagers());
    }
    private IEnumerator StartupManagers()
    {
        NetworkService network = new NetworkService();

        foreach(IGameManager manager in startSequence)
            manager.Startup(network);
        
        yield return null;

        int total = startSequence.Count;
        int started = 0;

        while (started < total)
        {
            int lastLoop = started;
            started = 0;

            foreach(IGameManager manager in startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                    started++;
            }
            if (started < lastLoop)
                Debug.Log($"Progress: {started}/{total}");
            yield return null;
        }

        Debug.Log("All managers started up");

    }
}
