using UnityEngine;

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