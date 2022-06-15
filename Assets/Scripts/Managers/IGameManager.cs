using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerStatus
{
    Shutdown, Initializating, Started
}

public interface IGameManager
{
    ManagerStatus Status { get; }

    void Startup(NetworkService service);
}
