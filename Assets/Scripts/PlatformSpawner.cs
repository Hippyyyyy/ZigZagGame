using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] Platform platformPrefab;
    [SerializeField] Transform lastPlatform;
    Vector3 lastPosition;
    Vector3 newPos;
    bool isStop;
    ObjectPool platformPool;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        platformPool = new ObjectPool(platformPrefab.gameObject, transform);
        lastPosition = lastPlatform.position;
        StartCoroutine(SpawnPlatformCoroutine());
    }
    private void Update()
    {

    }

    IEnumerator SpawnPlatformCoroutine()
    {
        while (!isStop)
        {
            SpawnPlatform();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void SpawnPlatform()
    {
        GeneratePosition();
        GameObject platformObj = platformPool.GetObjInPool();
        platformObj.gameObject.SetActive(true);
        platformObj.GetComponent<Platform>().SetUp();
        platformObj.GetComponent<Platform>().SetPool(platformPool);
        platformObj.transform.position = newPos;
        Debug.Log("Platform spawned at: " + newPos);
        lastPosition = newPos;
    }

    void GeneratePosition()
    {
        newPos = lastPosition;

        var randomPos = Random.Range(0, 2);

        if (randomPos > 0)
        {
            newPos.x += 2f;
        }
        else
        {
            newPos.z += 2f;
        }
        newPos.y = 0;
        //newPos.z += 3f;
    }
}
