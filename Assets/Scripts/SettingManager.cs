using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] tilePrefabs = null;

    private Transform playerTransform;

    private float spawnZ = -20f; //where exactly on Z we want to spawn object (X == Y == 0)
    private float spawnX = 9f;
    private static float tileLenght = 10.0f;
    private static float safeZone = 20.0f;
    private static int amnTilesOnScreen = 10;
    private int lastPrefabIndex = 0;

    private List<GameObject> activeTiles;

    void Start()
    {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            if (i < 5)
                SpawnTile(0);
            else
                SpawnTile();
        }

    }

    void Update()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLenght))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;
        if (prefabIndex == -1)
            go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        else
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;

        go.transform.SetParent(transform); //puts object inside of this method 
        go.transform.position = Vector3.forward * spawnZ + Vector3.right * spawnX;
        spawnZ += tileLenght;
        activeTiles.Add(go);
    }

    private void DeleteTile() //test
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if (tilePrefabs.Length <= 1)
            return 0;

        int randomIndex = lastPrefabIndex;

        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;

    }

}

