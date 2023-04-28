using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UsefulMethods;

public class Winpoint : MonoBehaviour
{
    [SerializeField] int delayInSeconds;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("\n Player has entered the win zone! | Reloading scene...");

        if (other.gameObject.CompareTag("Player"))
        {
            DelayTaskAsync(SceneManagerExtended.ReloadScene, delayInSeconds).AsTask();
            Time.timeScale = Mathf.Lerp(1, 0.55f, delayInSeconds);
        }
    }

    void Update()
    {
        // Debugging.
        if (Input.GetKeyDown(KeyCode.R)) DelayTaskAsync(SceneManagerExtended.ReloadScene, delayInSeconds).AsTask();
    }
}