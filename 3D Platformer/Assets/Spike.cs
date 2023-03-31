using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] float force = 5f;
    PlayerController player;
    Rigidbody playerRB;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        player.CurrentHealth -= 10;
        other.gameObject.GetComponent<PlayerController>().MyRigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        Debug.Log($"Player took damage from a spike! | New Health: {player.CurrentHealth}");
    }
}