using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    public void ReactToHit()
    {
        WanderingAI behavior = GetComponent<WanderingAI>();
        if (behavior != null)
        {
            behavior.SetAlive(false);
        }    
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;
        this.transform.Rotate(0, 0, -90);
        this.transform.position = new Vector3(x, y - 0.5f, z);

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }
}
