using UnityEngine;

public class Pickup : MonoBehaviour
{
    PlayerController player;
    SFXManager sfxManager;

    void Start()
    {
        player     = FindObjectOfType<PlayerController>();
        sfxManager = FindObjectOfType<SFXManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sfxManager.PlaySFX(sfxManager.pickupSFX);
            player.IsDashing = false;
            Destroy(gameObject);
        }
    }
}