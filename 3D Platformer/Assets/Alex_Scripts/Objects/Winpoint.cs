using Cysharp.Threading.Tasks;
using UnityEngine;
using static UsefulMethods;

public class Winpoint : MonoBehaviour
{
    [SerializeField] int delayInSeconds;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("\n Player has entered the win zone! | Reloading scene...");
        if (other.gameObject.CompareTag("Player"))
            DelayTaskAsync(SceneManagerExtended.ReloadScene, delayInSeconds).AsTask();
    }

    void Update()
    {
        // Debugging.
        if (Input.GetKeyDown(KeyCode.B))
        {
            DelayTaskAsync(SceneManagerExtended.ReloadScene, delayInSeconds).AsTask();
        }

    }
}