using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMeshController : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(playerController.transform.position);
    }
}
