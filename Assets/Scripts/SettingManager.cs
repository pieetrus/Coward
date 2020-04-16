using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] tilePrefabs = null;

    private Transform playerTransform;

    private float spawnZ = -10f; //where exactly on Z we want to spawn object (X == Y == 0)
    private float spawnX = 11.5f;
    private static float tileLenght = 15.0f;
    private static float safeZone = 15.0f;
    private static int amnTilesOnScreen = 7;
    private int lastPrefabIndex = 0;

    private List<GameObject> activeTiles;

    void Start()
    {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile(-1, 1);
            SpawnTile(-1, -1);
            
        }

    }

    void Update()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLenght))
        {
            SpawnTile(-1,1);
            SpawnTile(-1, -1);
            DeleteTile();
            DeleteTile();
        }
    }

    private void SpawnTile(int prefabIndex = -1, int pos = 0)
    {

            GameObject go;
            if (prefabIndex == -1)
                go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
            else
                go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;

            go.transform.SetParent(transform); //puts object inside of this method 
            go.transform.position = Vector3.forward * spawnZ + Vector3.right * pos * spawnX;

            activeTiles.Add(go);
        if (pos == -1)
        {
            spawnZ += tileLenght;
        }


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

