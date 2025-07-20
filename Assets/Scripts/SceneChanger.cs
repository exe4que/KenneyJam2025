using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using KenneyJam2025;
public class SceneChanger : MonoBehaviour
{


    private void OnEnable()
    {
        GlobalEvents.OnSceneChangeRequested += LoadScenes;
    }

    private void OnDisable()
    {
        GlobalEvents.OnSceneChangeRequested -= LoadScenes;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnSceneChangeRequested -= LoadScenes;
    }


    //Recieves the main scene name and the rest additive scenes as a list of strings 
    private void LoadScenes(string mainScene, List<string> additiveScenes)
    {
        SceneManager.LoadScene(mainScene);  //Loads Main scene

        foreach (string scene in additiveScenes)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);  //Loads additive scenes 
        }
    }
}
