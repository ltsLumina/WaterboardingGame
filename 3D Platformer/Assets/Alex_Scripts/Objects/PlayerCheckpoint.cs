using System;
using UnityEngine;

/// <summary>
/// THIS SCRIPT IS USED IN CONJUNCTION WITH THE CHECKPOINT SCRIPT.
/// PLACE THIS SCRIPT ON THE PLAYER IF IT IS NOT ALREADY THERE.
/// IT ALLOWS YOU TO SPAWN AT THE LAST CHECKPOINT WHEN YOU DIE.
/// </summary>
public class PlayerCheckpoint : MonoBehaviour
{
    [SerializeField] Vector3 spawnOffsetPosition = new(2, 2, 0);

    void Start()
    {
        switch (Checkpoint.HasCheckpoint)
        {
            case true:
                transform.position = Checkpoint.LastCheckpointPosition + spawnOffsetPosition;
                transform.rotation = Checkpoint.LastCheckpointRotation;
                break;

            default:
                Debug.Log("No checkpoint found.");
                break;
        }
    }
}