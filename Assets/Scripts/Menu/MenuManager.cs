using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KenneyJam2025;
public class MenuManager : MonoBehaviour
{

    [SerializeField] private float _delayBeforePlay = 0f;
    [SerializeField] private float _delayBeforeExit = 0f;
    [SerializeField] private float _delayBeforeCredits = 0f;

    [SerializeField] private Animator _transitionAnim;

    public GameObject CreditsPanel;

    private bool _canClick = true; //To check if a button was clicked or not

    [SerializeField] private string _mainScene;  //Scenes that get assigned in the inspectorr
    [SerializeField] private List<string> _scenesAdditive = new List<string>();
    


    void Start()
    {
        _canClick = true;
    }

    public void PlayAction()
    {
        if (!_canClick) return;
        _canClick = false;

        if (_transitionAnim != null)
        {
            _transitionAnim.SetTrigger("Transition");
        }
        else
        {
            Debug.Log("No Transition animator asigned");
        }

        Debug.Log("Play button pressed");


        StartCoroutine(Play(_delayBeforePlay));
    }

    //delays scene change to give room to animations
    IEnumerator Play(float delay)
    {

        yield return new WaitForSeconds(delay);

        //Load Additive scenes 
        GlobalEvents.OnSceneChangeRequested?.Invoke(_mainScene, _scenesAdditive);

        _canClick = true;

    }


    public void ExitAction()
    {
        if (!_canClick) return;
        _canClick = false;

        StartCoroutine(ExitAfter(_delayBeforeExit));
    }

    IEnumerator ExitAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit(); //exits aplication
        _canClick = true;

    }

    public void CreditsAction()
    {
        if (!_canClick) return;
        _canClick = false;

        StartCoroutine(CreditsAfter(_delayBeforeCredits));
    }

    IEnumerator CreditsAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        CreditsPanel.SetActive(true);
        _canClick = true;

    }
    public void CloseCredits()
    {
        CreditsPanel.SetActive(false);
        _canClick = true;

    }

}
