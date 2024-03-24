using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChuyenMap : MonoBehaviour
{
    public float delaySecond = 1;
    public string newSceneMap2 = "map 2";
    public string newSceneMap3 = "map 3";
    public string newSceneWin = "win";

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            ModeSelect();
        }
    }

    public void ModeSelect()
    {
        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(delaySecond);

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "map 1")
        {
            SceneManager.LoadScene(newSceneMap2); // Load "map 2" from "map 1"
        }
        else if (currentScene == "map 2")
        {
            SceneManager.LoadScene(newSceneMap3); // Load "map 3" from "map 2"
        }
        else if (currentScene == "map 3")
        {
            SceneManager.LoadScene(newSceneWin); // Load "win" from "map 3"
        }
    }
}
