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

        int numberOfObjects = 5;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0.1f, Mathf.Sin(angle)) * 5f;
            Instantiate(playerCharacter, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        /**
         * Was the CharacterController touching the ground during the last move?
         */
        if (controller.isGrounded)
        {
            /**
             * Returns whether the given mouse button is held down.
             */
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo)){
                    /**
                     * the player clicked on a Collider (any Collider)
                     *
                     * save the point that he wanted to move towards
                     */
                    moveTowards = hitInfo.point;
                }
            }
            if (!nearEnough(moveTowards, transform.position)) {
                /**
                 * if the player is further away from the position that he wanted to move towards, then we
                 * calculate relatively what direction that point is from the player, to try to move that way
                 */
                moveDirection = (moveTowards - transform.position).normalized;
            } else {
                /**
                 * if the player is at the point that he wanted to move towards, then we won't move relatively at all
                 */
                moveDirection = Vector3.zero;
            }
            /**
             * Transforms direction from local space to world space.
             */
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (moveTowards.y > transform.position.y) {
                /**
                 * if the player tries to move towards a position that is higher up then we don't want him to "jump" towards that higher point
                 *
                 * going on even ground or going down (into the ground) should not be problematic like that
                 */
                moveDirection.y = 0;
            }

            /**
             * Look in the direction that we are moving...
             */
            playerCharacter.transform.LookAt(moveTowards);
            /**
             * but do not look up or down
             */
            playerCharacter.transform.eulerAngles = new Vector3(0, playerCharacter.transform.eulerAngles.y, playerCharacter.transform.eulerAngles.z);
        }
        /**
         * keep moving even in the air
         */
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }

    bool nearEnough(Vector3 target, Vector3 currentPosition) {
        target.y = 0f;
        currentPosition.y = 0f;
        return Vector3.Distance(currentPosition, target) < 0.001f;
    }
}
