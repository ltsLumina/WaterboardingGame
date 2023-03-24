using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public static class SceneManagerExtended
{
    private static int previousScene = 0;
    static GameObject loadingScreen;
    static Slider slider;
    static private void Start()
    {
        loadingScreen = GameObject.Find("LoadingScreen");
        slider = loadingScreen.GetComponent<Slider>();
    }
    #region Static Methods
    /// <summary>
    /// Loads the scene with the specified build index.
    /// </summary>
    /// <param name="buildIndex"></param>
    public static void LoadScene(int buildIndex)
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(ClampBuildIndex(buildIndex));
    }

    /// <summary>
    /// Reloads the currently active scene.
    /// </summary>
    public static void ReloadScene()
    {
        SceneManager.LoadScene(ClampBuildIndex(SceneManager.GetActiveScene().buildIndex));
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Loads the next scene according to build index order.
    /// </summary>
    public static void LoadNextScene()
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(ClampBuildIndex(SceneManager.GetActiveScene().buildIndex + 1));
    }

    /// <summary>
    /// Loads the previously loaded scene.
    /// </summary>
    public static void LoadPreviousScene()
    {
        SceneManager.LoadScene(ClampBuildIndex(previousScene));
    }

    /// <summary>
    /// If the buildIndex is outside the range of build indexes, return 0.
    /// </summary>
    /// <param name="buildIndex"></param>
    /// <returns></returns>
    private static int ClampBuildIndex(int buildIndex)
    {
        if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("BuildIndex invalid/unavailable. Loading scene with an index of 0...");
            buildIndex = 0;
        }

        return buildIndex;
    }
    #endregion

    #region Static IEnumerators
    public static IEnumerator LoadingScreen()
    {
        LoadNextScene();
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            Debug.Log(operation.progress);

            yield return null;
        }
    }
    #endregion
}