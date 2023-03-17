using System;
using System.Collections;
using UnityEngine;
using static UsefulShortcuts;

public class Winpoint : MonoBehaviour
{
    SceneLoader loader;
    [SerializeField] int delayInSeconds;

    void Start() => loader = FindObjectOfType<SceneLoader>();

    IEnumerator OnTriggerEnter(Collider other)
    {
        Debug.Log("\n Player has entered the win zone! | Reloading scene...");
        if (!other.gameObject.CompareTag("Player")) yield break;
        DoAfterDelay(loader.ReloadScene, delayInSeconds);
        //TODO: ^warning.
    }

    void Update()
    {

        // Debugging.
        if (Input.GetKeyDown(KeyCode.B))
        {
            DoAfterDelay(() => loader.ReloadScene() , delayInSeconds);
        }

    }
}