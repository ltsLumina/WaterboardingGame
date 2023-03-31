using System;
using System.Collections;
using UnityEngine;

public class BreakableCrate : MonoBehaviour
{
    [SerializeField] GameObject particleEffect;
    [SerializeField] GameObject dustEffect;
    PlayerController playerRB;
    MeshRenderer meshRenderer;
    Collider collider;

    void Start()
    {
        playerRB     = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        meshRenderer = GetComponent<MeshRenderer>();
        collider     = GetComponent<BoxCollider>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            if (playerRB.IsDashing)
            {
                particleEffect.GetComponent<ParticleSystem>().Play();
                dustEffect           = Instantiate(dustEffect, transform.position, transform.rotation);
                meshRenderer.enabled = false;
                collider.enabled     = false;
                Destroy(gameObject, 1f);
            }
    }
}