using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] float force;
    [SerializeField] float damage;

    [Header("Cached References")]
    PlayerController player;
    Rigidbody playerRB;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerRB = player.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!player.IsDashing)
            {
               SpikeCollision();
            }
            else
            {
                player.IsDashing = false;
                SpikeCollision();
                playerRB.velocity = Vector3.zero;
            }
        }

        void SpikeCollision()
        {
            player.CurrentHealth -= damage;
            other.gameObject.GetComponent<PlayerController>().MyRigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
            Debug.Log($"Player took damage from a spike! \n New Health: {player.CurrentHealth}");
        }
    }
}