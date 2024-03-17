using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] GameObject birdPrefab;
    [SerializeField] float spawnInterval = 3.0f;
    [SerializeField] float spawnIntervalRandomness = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BirdSpawn");
    }

    // Update is called once per frame

    IEnumerator BirdSpawn()
    {
        while (true)
        {
            Instantiate(birdPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval + Random.Range(-spawnIntervalRandomness, spawnIntervalRandomness));
        }
    }
}
