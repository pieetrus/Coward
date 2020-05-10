using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
    private int settings_amount = 2;
    private int actual_setting = 0;
    private float setting_time = 30.0f;

    private List<GameObject> activeTiles;

    void Start()
    {
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            SpawnTile(1);
            SpawnTile(-1);
        }
        InvokeRepeating("ChangeSetting", setting_time, setting_time);
    }

    void Update()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLenght))
        {
            SpawnTile(1);
            SpawnTile(-1);
            DeleteTile();
            DeleteTile();
        }
        
    }

    private void SpawnTile(int pos = 0)
    {
            
            GameObject go;
            
            go = Instantiate(tilePrefabs[PrefabIndex()]) as GameObject;
            

            go.transform.SetParent(transform); //puts object inside of this method 
            go.transform.position = Vector3.forward * spawnZ + Vector3.right * pos * spawnX;
            go.transform.Rotate(0, UnityEngine.Random.Range(0, 4) * 90, 0);
            

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
            randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;

    }

    void ChangeSetting()
    {
        if(actual_setting+1 == settings_amount)
        {
            actual_setting = 0;
        }
        else { actual_setting++; }
    }

    private int PrefabIndex()
    {
        return UnityEngine.Random.Range(0 + actual_setting * 10, 10 + actual_setting * 10);
    }

}

