using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject playerCharacter;

    private Vector3 moveTowards = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    public float speed = 10.0f;
    public float gravity = 20.0f;
    void Start()
    {
        playerCharacter = GameObject.Find("playerCharacter");
        controller = GetComponent<CharacterController>();
        gameObject.transform.position = new Vector3(0, 5, 0);
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo)){
                    moveTowards = hitInfo.point;
                }
            }

            if (!nearEnough(moveTowards, transform.position)) {
                moveDirection = (moveTowards - transform.position).normalized;
            } else {
                moveDirection = Vector3.zero;
            }
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            playerCharacter.transform.LookAt(moveTowards);
            playerCharacter.transform.eulerAngles = new Vector3(0, playerCharacter.transform.eulerAngles.y, playerCharacter.transform.eulerAngles.z);
        }
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);


    }

    bool nearEnough(Vector3 target, Vector3 currentPosition) {
        target.y = 0f;
        currentPosition.y = 0f;
        return Vector3.Distance(currentPosition, target) < 0.001f;
    }
}
