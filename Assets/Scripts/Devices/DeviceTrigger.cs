using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] targets;

    public bool requireKey;

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject target in targets)
        {
            if (requireKey && Managers.Inventory.equippedItem != "key")
            {
                Debug.Log("You don't have the key equipped");
                return;
            }
            target.SendMessage("Activate");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject target in targets)
        {
            target.SendMessage("Deactivate");
        }    
    }
}
