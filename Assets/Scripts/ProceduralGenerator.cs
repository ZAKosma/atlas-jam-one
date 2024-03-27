using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeightedPrefab
{
    public GameObject prefab;
    public float weight;
}

public class ProceduralGenerator : MonoBehaviour
{
    public List<WeightedPrefab> weightedPrefabsToSpawn;
    public GameObject spawnArea; // Define a game object to represent the spawn area
    public float minHeight = 1.0f;
    public float maxHeight = 3.0f;
    public float rangeFromPlayerXZ = 10.0f; // Maximum distance from the player to spawn objects
    public float minDistanceFromPlayer = 5.0f; // Minimum distance from the player to spawn objects
    public float rangeFromAreaY = 5.0f; // Maximum distance from the spawn area's height to spawn objects
    public Vector2 minMaxScale = new Vector2(0.5f, 2.0f);
    public float spawnInterval = 1.0f; // Interval between spawns in seconds
    public string playerTag = "Player"; // Tag of the player object

    private List<GameObject> spawnablePrefabs;
    private GameObject playerObject;

    void Start()
    {
        spawnablePrefabs = new List<GameObject>();
        foreach (var weightedPrefab in weightedPrefabsToSpawn)
        {
            for (int i = 0; i < weightedPrefab.weight; i++)
            {
                spawnablePrefabs.Add(weightedPrefab.prefab);
            }
        }

        playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject == null)
        {
            Debug.LogWarning("Player object not found with tag: " + playerTag);
        }

        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            SpawnPrefab();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnPrefab()
    {
        if (spawnablePrefabs.Count == 0 || playerObject == null)
        {
            Debug.LogWarning("No spawnable prefabs or player object not found.");
            return;
        }

        // Randomly select a prefab
        int randomIndex = UnityEngine.Random.Range(0, spawnablePrefabs.Count);
        GameObject selectedPrefab = spawnablePrefabs[randomIndex];

       // Random position within the spawn area, with minimum distance from the player
        Vector3 spawnPosition = Vector3.zero;

        do
        {
            spawnPosition = playerObject.transform.position + new Vector3(
                UnityEngine.Random.Range(-rangeFromPlayerXZ, rangeFromPlayerXZ),
                0,
                UnityEngine.Random.Range(-rangeFromPlayerXZ, rangeFromPlayerXZ)
            );

            // Check if the spawn position is too close to the player
            if (Vector3.Distance(spawnPosition, playerObject.transform.position) < minDistanceFromPlayer)
            {
                // Adjust the spawn position
                float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2); // Random angle in radians
                float distance = minDistanceFromPlayer + UnityEngine.Random.Range(0.0f, rangeFromPlayerXZ); // Random distance within rangeFromPlayerXZ
                spawnPosition = playerObject.transform.position + new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);
            }

            // Clamp spawn position to the x and z values of the spawn area
            float halfSpawnAreaWidth = spawnArea.transform.localScale.x / 2;
            float halfSpawnAreaLength = spawnArea.transform.localScale.z / 2;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, spawnArea.transform.position.x - halfSpawnAreaWidth, spawnArea.transform.position.x + halfSpawnAreaWidth);
            spawnPosition.z = Mathf.Clamp(spawnPosition.z, spawnArea.transform.position.z - halfSpawnAreaLength, spawnArea.transform.position.z + halfSpawnAreaLength);
        } while (Vector3.Distance(spawnPosition, playerObject.transform.position) < minDistanceFromPlayer);

        // Set the y position to the spawn area's height plus or minus a small range
        spawnPosition.y = spawnArea.transform.position.y + UnityEngine.Random.Range(-rangeFromAreaY, rangeFromAreaY);


        // Random rotation clamped between 0 and 45 degrees
Quaternion spawnRotation = Quaternion.Euler(UnityEngine.Random.Range(0, 45), UnityEngine.Random.Range(0, 45), UnityEngine.Random.Range(0, 45));

        // Random scale within the specified range
        float randomScale = UnityEngine.Random.Range(minMaxScale.x, minMaxScale.y);

        // Instantiate the prefab with random position, rotation, and scale
        GameObject spawnedObject = Instantiate(selectedPrefab, spawnPosition, spawnRotation);
        spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}