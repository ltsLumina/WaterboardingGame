using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameController : MonoBehaviour
{
    GameObject loadingScreen;
    GameObject slider;
    private void Start()
    {
        loadingScreen = GameObject.Find("LoadingScreen");
        slider = loadingScreen.transform.Find("Slider").gameObject;
    }
    #region
    public void LoadNext()
    {
        SceneManagerExtended.LoadNextScene();
        //StartCoroutine(SceneManagerExtended.LoadingScreen());
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
    /*IEnumerator LoadingScreen()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);
        while (!operation.isDone) 
        {
            Debug.Log(operation.progress);

            yield return null;
        }
    }*/
}
