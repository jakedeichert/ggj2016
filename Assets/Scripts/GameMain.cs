using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour {
    public EnemyBehaviour enemyPrefab;
    public GameObject player;
    List<EnemyBehaviour> allEnemies = new List<EnemyBehaviour>();
    public int numEnemies = 3;

    private Camera camera;
    public Image fadePanel;


    void Start() {
        //Scene Fade in start code
        if (fadePanel != null) {
            Color fadeColor = fadePanel.color;
            fadeColor.a = 1.0f;
            fadePanel.color = fadeColor;
            StartCoroutine(FadeOut());
        }
        //Enemy start code
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

    void RestartLevel() {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() {
        while (fadePanel.color.a <= 1.0f) {
            Color fadeColor = fadePanel.color;
            fadeColor.a += Time.deltaTime;
            //Debug.Log(fadeColor.a);
            fadePanel.color = fadeColor;
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        yield return null;
    }

    IEnumerator FadeOut() {
        while (fadePanel.color.a > 0.0f) {
            Color fadeColor = fadePanel.color;
            fadeColor.a -= Time.deltaTime;
            //Debug.Log(fadeColor.a);
            fadePanel.color = fadeColor;
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
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
