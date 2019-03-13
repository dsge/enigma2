using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject playerCharacter;
    Vector3? walkingTowards;
    void Start()
    {
        playerCharacter = GameObject.Find("playerCharacter");
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)){
                //playerCharacter.transform.lossyScale.y
                playerCharacter.transform.LookAt(new Vector3(hitInfo.point.x, 0.5f, hitInfo.point.z));
                walkingTowards = hitInfo.point;
            }
        }

        if (walkingTowards != null) {
            if (nearEnough((Vector3)walkingTowards, transform.position)){
                walkingTowards = null;
            } else {
                transform.position = Vector3.MoveTowards(transform.position, (Vector3)walkingTowards, 10.0f * Time.deltaTime);
            }
        }


    }

    bool nearEnough(Vector3 target, Vector3 currentPosition) {
        target.y = 0f;
        currentPosition.y = 0f;
        return Vector3.Distance(currentPosition, target) < 0.001f;
    }
}
