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

    private void LoadScenes(string mainScene, List<string> additiveScenes)
    {
        SceneManager.LoadScene(mainScene);  // Carga principal

        foreach (string scene in additiveScenes)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);  // Carga additiva
        }
    }
}
