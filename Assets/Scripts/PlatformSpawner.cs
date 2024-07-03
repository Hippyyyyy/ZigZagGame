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
    private void Start()
    {
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
            yield return new WaitForSeconds(0.2f);
        }
    }

    void SpawnPlatform()
    {
        GeneratePosition();
        Instantiate(platformPrefab, newPos, Quaternion.identity);
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

        //newPos.z += 3f;
    }
}
