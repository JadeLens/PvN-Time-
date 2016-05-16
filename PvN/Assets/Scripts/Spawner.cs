using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject Enemy;
    float lastSpawnTime;
    float totalTime;
    public float dropRate = 0.25f;
    public float SdropRate = 0.10f;
    public static int numEnemies;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lastSpawnTime += Time.deltaTime;
        totalTime += Time.deltaTime;

        if (lastSpawnTime >= 10 && numEnemies <= 3)
        {
            //Instantiate(Enemy, transform.position, transform.rotation);
            Instantiate(Enemy);
            numEnemies++;
            lastSpawnTime = 0;
        }

    }
}
