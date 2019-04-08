using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public float movementSpeed = 7.5f;
    public float minDistance = 1;
    public float maxDistance = 10;
    public float actualDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

        actualDistance = Vector3.Distance(transform.position, player.position);

        if (actualDistance >= minDistance && actualDistance <= maxDistance)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
    }
}
