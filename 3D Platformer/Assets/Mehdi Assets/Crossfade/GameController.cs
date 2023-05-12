using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : SingletonPersistent<GameController>
{
    Crossfade crossfade;

    private void Start()
    {
        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        crossfade = GameObject.FindObjectOfType<Crossfade>();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadNextScene()
    {
        crossfade.StartCoroutine(crossfade.LoadCrossfade());
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
