using System;
using UnityEngine;

/// <summary>
/// Checkpoint system when player touches this object,
/// it will save the player's position and rotation and when the player dies,
/// it will respawn at the last checkpoint touched.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    public static Vector3 LastCheckpointPosition { get; private set; }
    public static Quaternion LastCheckpointRotation { get; private set; }
    public static bool HasCheckpoint { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        LastCheckpointPosition = gameObject.transform.position;
        LastCheckpointRotation = other.transform.rotation;
        HasCheckpoint          = true;
        Debug.Log("Checkpoint saved!");

        //TODO: Play checkpoint sound and particle effect!
    }

    void Update() { Debug.Log(LastCheckpointPosition); }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}