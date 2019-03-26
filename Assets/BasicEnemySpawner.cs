using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemySpawner : MonoBehaviour
{

    const int LAYER_GROUND = 1 << 9;
    const int LAYER_ENEMIES = 1 << 10;

    protected List<GameObject> spawnedEnemies = new List<GameObject>();
    void Start()
    {
        GameObject enemyTemplate = Resources.Load("Enemy") as GameObject;

        int numberOfObjects = 5;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0.1f, Mathf.Sin(angle)) * 5f;
            GameObject enemy = Instantiate (enemyTemplate, pos, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }

    }

    public void removeEnemy(GameObject enemy) {
        if (spawnedEnemies.Contains(enemy)) {
            /**
             * destroy the gameobject
             */
            Destroy(enemy);
            /**
             * remove it from out list as well
             */
            spawnedEnemies.Remove(enemy);
        }
    }

    public List<GameObject> getEnemies() {
        return spawnedEnemies;
    }

    void Update()
    {

    }
}
