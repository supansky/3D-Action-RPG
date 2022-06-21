using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    private NetworkService network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager starting...");

        network = service;

        Health = 50;
        MaxHealth = 100;

        Status = ManagerStatus.Started;
    }
    public void ChangeHealth(int value)
    {
        Health += value;
        if(Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        else if(Health < 0)
        {
            Health = 0;
        }

        Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
    }
}
