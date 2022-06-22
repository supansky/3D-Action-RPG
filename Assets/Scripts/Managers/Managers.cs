using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (InventoryManager))]
[RequireComponent(typeof (PlayerManager))]
[RequireComponent(typeof(MissionManager))]


public class Managers : MonoBehaviour
{
    public static InventoryManager Inventory { get; private set; }
    public static PlayerManager Player { get; private set; }

    public static MissionManager Mission { get; private set; }

    private List<IGameManager> startSequence;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Inventory = GetComponent<InventoryManager>();
        Player = GetComponent<PlayerManager>();
        Mission = GetComponent<MissionManager>();

        startSequence = new List<IGameManager>();
        startSequence.Add(Inventory);
        startSequence.Add(Player);
        startSequence.Add(Mission);

        StartCoroutine(StartupManagers());
    }
    private IEnumerator StartupManagers()
    {
        NetworkService network = new NetworkService();

        foreach(IGameManager manager in startSequence)
            manager.Startup(network);
        
        yield return null;

        int numModules = startSequence.Count;
        int numReady = 0;

        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach(IGameManager manager in startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                    numReady++;
            }
            if (numReady > lastReady)
            {
                Debug.Log($"Progress: {numReady}/{numModules}");
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
            }
            yield return null;
        }

        Debug.Log("All managers started up");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);

    }
}
