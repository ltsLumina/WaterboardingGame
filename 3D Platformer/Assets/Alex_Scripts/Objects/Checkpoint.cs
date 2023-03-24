using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    /// <summary>
    /// Checkpoint system when player touches this object, it will save the player's position and rotation and when the player dies, it will respawn at the last checkpoint touched.
    /// </summary>

    static Vector3 lastCheckpointPosition;
    static Quaternion lastCheckpointRotation;
    static bool hasCheckpoint;

    public static Vector3 LastCheckpointPosition => lastCheckpointPosition;
    public static Quaternion LastCheckpointRotation => lastCheckpointRotation;
    public static bool HasCheckpoint => hasCheckpoint;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        lastCheckpointPosition = other.transform.position;
        lastCheckpointRotation = other.transform.rotation;
        hasCheckpoint          = true;
        Debug.Log("Checkpoint saved!");
    }

    void Update()
    {
        DEBUG_ResetCheckpoint();
    }

    void DEBUG_ResetCheckpoint()
    {
        if (!Input.GetKeyDown(KeyCode.C)) return;
        hasCheckpoint = false;
        Debug.Log("Checkpoint reset.");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}