using UnityEngine;

/// <summary>
/// Kills player if they are below the death plane position.
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class DeathFromBelow : MonoBehaviour
{
    [SerializeField] Vector3 deathplanePos;
    [SerializeField] GameObject deathOverlay;

    // Cached References
    PlayerController player;

    void Start() => player = FindObjectOfType<PlayerController>();

    void Update() { CheckForPlayer(); }

    void CheckForPlayer()
    {
        PlayerBelowDeathplane();

        void PlayerBelowDeathplane()
        {
            if (player.transform.position.y < deathplanePos.y)
            {
                Debug.Log("Player is below death plane, starting death sequence...");
                player.CurrentHealth = 0;

                if (deathOverlay != null) deathOverlay.SetActive(true);
            }
        }
    }
}
