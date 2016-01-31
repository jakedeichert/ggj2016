using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour {
    public EnemyBehaviour enemyPrefab;
    public GameObject player;
    List<EnemyBehaviour> allEnemies = new List<EnemyBehaviour>();
    public int numEnemies = 3;


    void Start() {
        GameObject enemiesHolder = new GameObject("enemies");
        // Create enemies.
        for (int i = 0; i < numEnemies; i++) {
            EnemyBehaviour e = Instantiate(enemyPrefab) as EnemyBehaviour;
            e.transform.parent = enemiesHolder.transform;
            e.playerTarget = player.transform;
            e.allEnemies = allEnemies;
            allEnemies.Add(e);
        }

        reset();
    }

    void Update() {

    }



    void reset() {
        // Randomize position.
        foreach (EnemyBehaviour e in allEnemies) {
            e.transform.position = new Vector3(
                Random.Range(player.transform.position.x - 10f, player.transform.position.x + 10f), 
                Random.Range(player.transform.position.y - 10f, player.transform.position.y + 10f),
                0);
        }
    }
}
