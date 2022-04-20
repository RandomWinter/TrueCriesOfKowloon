using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string stageName;

    public void Awake()
    {
        var currentStage = SceneManager.GetActiveScene();
        stageName = currentStage.name;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(stageName)
        {
            case "level 1-1":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(3);
                other.transform.position = new Vector2(44f, 5f);
            }
            break;

            case "level 1-2":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(4);
                other.transform.position = new Vector2(2f, 7f);
            }
            break;

            case "lvl 1-3":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(5);
                other.transform.position = new Vector2(44f, 5f);
            }
            break;

            case "lvl 2 - 1":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(6);
                other.transform.position = new Vector2(2f, 7f);
            }
            break;

            case "lvl 2 - 2":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(7);
                other.transform.position = new Vector2(44f, 5f);
            }
            break;

            case "lvl 2 - 3":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(8);
                other.transform.position = new Vector2(2f, 7f);
            }
            break;

            case "lvl 3 - 1":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(9);
                other.transform.position = new Vector2(44f, 5f);
            }
            break;

            case "lvl 3 - 2":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(10);
                other.transform.position = new Vector2(2f, 7f);
            }
            break;

            case "lvl 3 - 3":
            if(other.CompareTag("Player"))
            {
                SceneManager.LoadScene(11);
                other.transform.position = new Vector2(44f, 5f);
            }
            break;
        }
    }
}
