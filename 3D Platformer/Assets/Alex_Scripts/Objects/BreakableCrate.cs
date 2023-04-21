using System;
using System.Collections;
using UnityEngine;

public class BreakableCrate : MonoBehaviour
{
    [Header("Breakable Crate Settings")]
    [SerializeField, Tooltip("The time to wait before destroying all particles that spawn when the crate brakes.")]
    float lifetime;

    [Header("Cached References")]
    ParticleSystem particleEffect;
    ParticleSystem dustEffect;
    PlayerController playerRB;
    MeshRenderer meshRenderer;
    Collider boxCollider;

    void Start()
    {
        particleEffect = transform.GetChild(0).GetComponent<ParticleSystem>();
        dustEffect     = transform.GetChild(1).GetComponent<ParticleSystem>();
        playerRB       = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        meshRenderer   = GetComponent<MeshRenderer>();
        boxCollider    = GetComponent<BoxCollider>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && playerRB.IsDashing)
        {
            particleEffect.Play();
            dustEffect.Play();
            meshRenderer.enabled = false;
            boxCollider.enabled  = false;
            Destroy(gameObject, lifetime);
        }
    }
}