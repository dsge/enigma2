using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;

    private float movementSpeed = 2.5f;
    private float minDistance = 1;
    private float maxDistance = 5;
    private float actualDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(BasicSceneSwitchHandler.GLOBAL_COMPONENTS_HANDLER_NAME).GetComponent<BasicSceneSwitchHandler>().getPlayer().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.Warp(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        actualDistance = Vector3.Distance(transform.position, player.position);

        if (actualDistance >= minDistance && actualDistance <= maxDistance)
        {
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }
}
