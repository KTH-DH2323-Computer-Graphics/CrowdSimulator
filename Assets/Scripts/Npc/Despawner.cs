using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public string despawnTag = "Despawner";
    private void OnTriggerEnter(Collider other)
    {
        // Despawn if colliding object has the despawn tag
        if (other.CompareTag(despawnTag)) {
            Destroy(this.gameObject);
        }
    }
}
