using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        if(player.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        GameObject[] camera = GameObject.FindGameObjectsWithTag("Camera");

        if(camera.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        GameObject[] mainCamera = GameObject.FindGameObjectsWithTag("MainCamera");

        if(mainCamera.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        GameObject[] cameraBoundary = GameObject.FindGameObjectsWithTag("CameraBoundary");

        if(cameraBoundary.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
