using UnityEngine;

public class DeathFromBelow : MonoBehaviour
{
    [SerializeField] Vector3 deathplanePos;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update() { CheckForPlayer(); }
    void CheckForPlayer()
    {
        if (player.transform.position.y < deathplanePos.y)
        {
            Debug.Log("Player is below death plane");
            player.CurrentHealth = 0;
        }
    }
}