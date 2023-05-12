using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    /// <summary>
    /// Checkpoint system when player touches this object, it will save the player's position and rotation and when the player dies, it will respawn at the last checkpoint touched.
    /// </summary>
    public static Vector3 LastCheckpointPosition { get; private set; }
    public static Quaternion LastCheckpointRotation { get; private set; }
    public static bool HasCheckpoint { get; private set; }

#if UNITY_EDITOR
    void Awake()
    {
        HasCheckpoint = false;
    }
#endif

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        LastCheckpointPosition = other.transform.position;
        LastCheckpointRotation = other.transform.rotation;
        HasCheckpoint          = true;
        Debug.Log("Checkpoint saved!");

        //TODO: Play checkpoint sound and particle effect!
    }

#if UNITY_EDITOR
    void Update() => DEBUG_ResetCheckpoint();

    void DEBUG_ResetCheckpoint()
    {
        if (!Input.GetKeyDown(KeyCode.C)) return;
        HasCheckpoint = false;
        Debug.Log("Checkpoint reset.");
    }
#endif

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}