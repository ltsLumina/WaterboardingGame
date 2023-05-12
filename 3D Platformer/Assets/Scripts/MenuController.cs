using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1;

    public static bool gameIsPaused = false;
    #region
    public void LoadNext()
    {
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadingScreen(sceneToLoad + 1));
    }
    public void Reload()
    {
        SceneManagerExtended.ReloadScene();
        Time.timeScale = 1.0f;
    }
    public void LoadPrevious()
    {
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadingScreen(sceneToLoad - 1));
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit App");
    }
    #endregion

    private void OnPause()
    {
        //If game is not paused, pause and show screen.
        if (!gameIsPaused)
        {
            Time.timeScale = 0f;
            gameIsPaused = true;
            pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        //If game is  paused, unpause and hide screen.
        else if (gameIsPaused)
        {
            Time.timeScale = 1f;
            gameIsPaused = false;
            pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Resume(GameObject gameObject)
    {
        Time.timeScale = 1.0f;
        gameIsPaused = false;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public IEnumerator LoadingScreen(int scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        if (operation.isDone)
        {
            Time.timeScale = 1.0f;
        }
    }
}
