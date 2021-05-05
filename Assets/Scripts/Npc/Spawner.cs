using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public NPCMovement npcPrefab;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2f;
    public Vector3 spawnSize = new Vector3(10, 0, 10);
    public Vector3 npcWalkingDirection = new Vector3(0, 0, 1);
    public float minSpeed = 1.0f;
    public float maxSpeed = 1.0f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }

    void Spawn()
    {   
        // Create new NPC gameObject
        Vector3 spawnPosition = this.gameObject.transform.position + Random.Range(-0.5f, 0.5f) * spawnSize;
        NPCMovement npc = Instantiate(npcPrefab, spawnPosition, this.gameObject.transform.rotation);

        npc.SetWalkingData(this.maxSpeed, npcWalkingDirection);
                
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
