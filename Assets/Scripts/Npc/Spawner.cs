using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2f;
    public Vector3 verticalSpawnDirection = Vector3.right;
    public float verticalSpawnRange = 3f;
    void Spawn()
    {   
        // Create new NPC gameObject
        GameObject npc = Instantiate(npcPrefab, this.gameObject.transform, false);

        // Randomize vertical spawn position from spawnpoint
        Vector3 verticalSpawnPosition = verticalSpawnDirection * Random.Range(-verticalSpawnRange, verticalSpawnRange);
        Transform newTransform = npc.transform;
        newTransform.position += verticalSpawnPosition;
                
        //Randomize new spawntime based on user parameters
        float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("Spawn", randomTime);
    }
    void Start()
    {
        // Initial spawn
        Invoke("Spawn", 0.5f);
    }
}
