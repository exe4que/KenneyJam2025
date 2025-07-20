using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private float _delayBeforePlay = 0f;
    [SerializeField] private float _delayBeforeExit = 0f;
    [SerializeField] private float _delayBeforeCredits = 0f;

    [SerializeField] private Animator _transitionAnim;

    public GameObject CreditsPanel;

    private bool canClick = true; //To check if a button was clicked or not

    

    void Start()
    {
        canClick = true;
    }

    public void PlayAction()
    {
        if (!canClick) return;
        canClick = false;

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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //Changes to next scene 
        canClick = true;

    }


    public void ExitAction()
    {
        if (!canClick) return;
        canClick = false;

        StartCoroutine(ExitAfter(_delayBeforeExit));
    }

    IEnumerator ExitAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit(); //exits aplication
        canClick = true;

    }

    public void CreditsAction()
    {
        if (!canClick) return;
        canClick = false;

        StartCoroutine(CreditsAfter(_delayBeforeCredits));
    }

    IEnumerator CreditsAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        CreditsPanel.SetActive(true);
        canClick = true;

    }
    public void CloseCredits()
    {
        CreditsPanel.SetActive(false);
        canClick = true;

    }

}
