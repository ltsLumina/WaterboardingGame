using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    void Start()
    {
        if (Checkpoint.HasCheckpoint)
        {
            transform.position = Checkpoint.LastCheckpointPosition;
            transform.rotation = Checkpoint.LastCheckpointRotation;
        }
        else
        {
            Debug.Log("No checkpoint found.");
        }
    }
}