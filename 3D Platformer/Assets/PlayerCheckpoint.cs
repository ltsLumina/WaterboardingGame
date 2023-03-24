using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    void Start()
    {
        switch (Checkpoint.HasCheckpoint)
        {
            case true:
                transform.position = Checkpoint.LastCheckpointPosition;
                transform.rotation = Checkpoint.LastCheckpointRotation;
                break;

            default:
                Debug.Log("No checkpoint found.");
                break;
        }
    }
}