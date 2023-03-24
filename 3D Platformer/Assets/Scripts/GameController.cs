using UnityEngine;

public class GameController : MonoBehaviour
{
    public void LoadNext()
    {
        SceneManagerExtended.LoadNextScene();
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
}
