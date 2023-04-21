using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    [SerializeField] Vector3 offsetPosition = new(2, 2, 0);

    void Start()
    {
        switch (Checkpoint.HasCheckpoint)
        {
            case true:
                transform.position = Checkpoint.LastCheckpointPosition + offsetPosition;
                transform.rotation = Checkpoint.LastCheckpointRotation;
                break;

            default:
                Debug.Log("No checkpoint found.");
                break;
        }
    }
}