using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Buttons : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RetryCall()
    {
        // .. then reload the currently loaded level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitCall()
    {
        SceneManager.LoadScene(ARK.Managers.SceneManager.Names.MainMenu);
    }
}


