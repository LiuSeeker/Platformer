using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject;
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(cam);
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLevel(string newLevel)
    {
        
        Scene actualLevel = SceneManager.GetActiveScene();
        string spawn;

        if(actualLevel.name == "Test Scene 1" && newLevel == "Test Scene 2")
        {
            spawn = "Spawn1";
        }
        else if(actualLevel.name == "Test Scene 2" && newLevel == "Test Scene 1")
        {
            spawn = "Spawn1";
        }
        else if(actualLevel.name == "level0" && newLevel == "level1")
        {
            spawn = "Spawn1-1";
        }
        else if (actualLevel.name == "level1" && newLevel == "level2")
        {
            spawn = "Spawn2-1";
        }
        else if (actualLevel.name == "level2" && newLevel == "level1")
        {
            spawn = "Spawn1-2";
        }
        else if (actualLevel.name == "level2" && newLevel == "level3")
        {
            spawn = "Spawn3-1";
        }
        else if (actualLevel.name == "level3" && newLevel == "level2")
        {
            spawn = "Spawn2-2";
        }
        else if (actualLevel.name == "level2" && newLevel == "level4")
        {
            spawn = "Spawn4-1";
        }
        else if (actualLevel.name == "level4" && newLevel == "level2")
        {
            spawn = "Spawn2-3";
        }
        else
        {
            spawn = "";
        }


        StartCoroutine(LoadLevel(newLevel, spawn));
        
    }


    private IEnumerator LoadLevel(string newLevel, string spawn)
    {
        AsyncOperation levelLoad = SceneManager.LoadSceneAsync(newLevel);
        Canvas canvas;
        
        while (!levelLoad.isDone)
        {
            yield return null;
        }

        Scene actualLevel = SceneManager.GetActiveScene();
        GameObject[] sceneGO = actualLevel.GetRootGameObjects();
        for (int i = 0; i < sceneGO.Length; i++)
        {
            if (sceneGO[i].name == spawn)
            {
                player.transform.position = sceneGO[i].transform.position;
            }
            else if(sceneGO[i].name == "Canvas")
            {
                canvas = sceneGO[i].GetComponent<Canvas>();
                canvas.worldCamera = cam.GetComponent<Camera>();
            }
            else if (sceneGO[i].tag == "LevelWarp")
            {
                sceneGO[i].GetComponent<LevelWarpScript>().gm = this;
            }
        }

    }
}
