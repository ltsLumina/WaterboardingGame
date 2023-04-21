using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider slider;

    public static bool gameIsPaused = false;
    #region
    public void LoadNext()
    {
        StartCoroutine(LoadingScreen());
    }
    public void Reload()
    {
        SceneManagerExtended.ReloadScene();
    }
    public void LoadPrevious()
    {
        SceneManagerExtended.LoadPreviousScene();
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit App");
    }
    #endregion

    private void OnPause()
    {
        if (!gameIsPaused)
        {
            gameIsPaused = true;
            GameObject pauseScreen = GameObject.Find("PauseScreen");
            pauseScreen.SetActive(true);
        }
        else if (gameIsPaused)
        {
            gameIsPaused = false;
            GameObject pauseScreen = GameObject.Find("PauseScreen");
            pauseScreen.SetActive(false);

        }
    }
    public IEnumerator LoadingScreen()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);
        while (!operation.isDone) 
        {
            //Debug.Log(operation.progress);
            slider.value = operation.progress;

            yield return null;
        }
    }
}
