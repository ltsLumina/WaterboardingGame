using UnityEngine;

public class BreakableCrate : MonoBehaviour
{
    [Header("Breakable Crate Settings")]
    [Range(1,5), Tooltip("The time to wait before destroying all particles that spawn when the crate brakes.")]
    [SerializeField] float lifetime;

    [Header("Cached References")]
    ParticleSystem particleEffect;
    ParticleSystem dustEffect;
    PlayerController player;
    MeshRenderer meshRenderer;
    Collider boxCollider;

    void Start()
    {
        particleEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
        dustEffect     = transform.GetChild(1).GetComponent<ParticleSystem>();
        player       = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        meshRenderer   = GetComponent<MeshRenderer>();
        boxCollider    = GetComponent<BoxCollider>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && player.IsDashing)
        {
            particleEffect.Play();
            dustEffect.Play();
            meshRenderer.enabled = false;
            boxCollider.enabled  = false;

            //Debug.Assert(lifetime > 0, $"Lifetime is {lifetime} which is less than 0! Please set a positive value.");
            Destroy(gameObject, lifetime);
        }
    }
}