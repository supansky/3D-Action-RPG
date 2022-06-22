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

        UpdateData(50, 100);

        Status = ManagerStatus.Started;
    }

    public void UpdateData(int health, int maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
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

        if (Health == 0)
        {
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }

        Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
    }
    public void Respawn()
    {
        UpdateData(50, 100);
    }
}
