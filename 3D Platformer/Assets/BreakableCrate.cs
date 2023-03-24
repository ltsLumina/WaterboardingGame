using System;
using UnityEngine;

public class BreakableCrate : MonoBehaviour
{
    PlayerController playerRB;
    [SerializeField] GameObject particleEffect;

    void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    //TODO: refactor
    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (playerRB.IsDashing)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }
    }
}