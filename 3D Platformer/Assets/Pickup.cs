using UnityEngine;

public class Pickup : MonoBehaviour
{
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.IsDashing = false;
            Destroy(gameObject);
        }
    }
}