using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemySpawner : MonoBehaviour
{
    protected List<GameObject> spawnedEnemies = new List<GameObject>();
    void Start()
    {
        spawnDefaultEnemies();
    }

    protected void spawnDefaultEnemies() {
        /**
         * this is the enemy that we will duplicate
         */
        GameObject enemyTemplate = Resources.Load("BasicEnemy") as GameObject;

        int numberOfEnemiesToSpawn = 5;
        /**
         * the enemies should not spawn around the middle of the map, they should instead spawn around this point
         */
        Vector3 spawnCircleOffset = new Vector3(2, 0, 2);
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            /**
             * spawn enemies in a circle, with even spacing between them
             */
            float angle = i * Mathf.PI * 2 / numberOfEnemiesToSpawn;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0.1f, Mathf.Sin(angle)) * 5f;
            GameObject enemy = Instantiate (enemyTemplate, pos + spawnCircleOffset, Quaternion.identity);
            enemy.AddComponent(typeof(Health));
            enemy.AddComponent(typeof(NavMeshAgent));
            /**
             * we need to set the NavMeshAgent's height and baseOffset to stop the enemy sunk in the ground
             */
            enemy.GetComponent<NavMeshAgent>().height = 1.0f;
            enemy.GetComponent<NavMeshAgent>().baseOffset = 1.0f;
            /**
             * we need to store a reference to the spawned enemy to keep track of him later
             */
            spawnedEnemies.Add(enemy);
        }
    }
    /**
     * this should be called to "kill" the enemy
     */
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
    public void removeAllEnemies() {
        spawnedEnemies.ForEach(delegate(GameObject enemy) {
            Destroy(enemy);
        });
        spawnedEnemies.Clear();
    }
    /**
     * what enemies are currently on the map?
     */
    public List<GameObject> getEnemies() {
        return spawnedEnemies;
    }
}
