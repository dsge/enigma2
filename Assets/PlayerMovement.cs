using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject playerCharacter;
    /**
     * the point on the ground that the player intends to move to
     *
     * this implicitly means that we are not walking towards any specific GameObject
     */
    private Vector3 moveTowardsPointOnGround;
    /**
     * the gameobject that we are moving towards (e.g. chasing this enemy or moving towards this door)
     *
     * this implicitly means that we are not walking towards a random point on the ground
     */
    private GameObject moveTowardsGameObject;

    private Vector3 moveDirectionInPreviousFrame = Vector3.zero;
    private CharacterController controller;

    private BasicEnemySpawner spawner;
    private EnemyHpTopbarDisplay enemyHpTopBarDisplay;

    private Animator animator;

    public float speed = 10.0f;
    public float gravity = 20.0f;
    void Start()
    {
        playerCharacter = GameObject.Find("playerCharacter");
        controller = GetComponent<CharacterController>();
        gameObject.transform.position = new Vector3(0, 5, 0);

        spawner = GameObject.Find("global components handler").GetComponent<BasicEnemySpawner>();
        enemyHpTopBarDisplay = GameObject.Find("global components handler").GetComponent<EnemyHpTopbarDisplay>();

        animator = GameObject.Find("Weapon").GetComponent<Animator>();
    }
    /**
     * which direction should the player move in this frame?
     *
     * @param Vector3 targetPoint the global point that we want to move towards
     */
    protected Vector3 calculateMoveDirection(Vector3 targetPoint) {
        if (!controller.isGrounded) {
            return moveDirectionInPreviousFrame;
        }
        Vector3 moveDirection;
        if (!nearEnough(targetPoint, transform.position, 0.01f)) {
            /**
            * if the player is further away from the position that he wanted to move towards, then we
            * calculate relatively what direction that point is from the player, to try to move that way
            */
            moveDirection = (targetPoint - transform.position).normalized;
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

        if (targetPoint.y > transform.position.y) {
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
        playerCharacter.transform.LookAt(targetPoint);
        /**
        * but do not look up or down
        */
        playerCharacter.transform.eulerAngles = new Vector3(0, playerCharacter.transform.eulerAngles.y, playerCharacter.transform.eulerAngles.z);
        return moveDirection;
    }

    void Update()
    {
        moveTowardsGameObject = calculateObjectToMoveTowards();
        moveTowardsPointOnGround = calculatePointOnGroundToMoveTowards();

        Vector3 moveDirection = moveDirectionInPreviousFrame;
        moveDirection.x = 0;
        moveDirection.z = 0;

        if (!moveTowardsPointOnGround.Equals(Vector3.zero)){
            //moveDirection = calculateMoveDirection(moveTowardsPointOnGround);
        }
        if (moveTowardsGameObject) {
            moveDirection = calculateMoveDirection(moveTowardsGameObject.transform.position);
        }
        /**
         * keep moving even in the air
         */
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
        moveDirectionInPreviousFrame = moveDirection;
        colorEnemiesOnHover();
    }

    GameObject calculateObjectToMoveTowards() {
        if (!controller.isGrounded) {
            return null;
        }
        if (moveTowardsGameObject != null) {
            if (Input.GetMouseButtonUp(0)) {
                return null;
            } else {
                return moveTowardsGameObject;
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100000f, Layers.ENEMIES)){
                /**
                * the player clicked on an enemy
                *
                * save the point that he wanted to move towards
                */
                return hitInfo.transform.parent.gameObject;

                /* if (nearEnough(moveTowards, transform.position, 2f)) {
                    GameObject enemy = hitInfo.transform.parent.gameObject;
                    hitEnemy(enemy);
                }*/
            }
        }
        return null;
    }
    Vector3 calculatePointOnGroundToMoveTowards() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100000f, Layers.GROUND)){
            /**
            * the player clicked on a Collider (any Collider)
            *
            * save the point that he wanted to move towards
            */
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    void hitEnemy(GameObject enemy) {
        animator.SetTrigger("Attack");
        Health enemyHealth = enemy.GetComponent<Health>();
        enemyHealth.damageFor(10);
        if (enemyHealth.isDead()) {
            spawner.removeEnemy(enemy);
        }
    }

    void colorEnemiesOnHover() {
        /**
         * turn all enemies into the default color...
         */
        foreach (var enemy in spawner.getEnemies()) {
            enemy.transform.Find("Cylinder").GetComponent<Renderer>().material.color = new Color(255, 255, 255);
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100000f, Layers.ENEMIES)){
            GameObject enemy = hitInfo.transform.parent.gameObject;
            /**
            * ...but turn the one we are pointing at green...
            */
            enemy.transform.Find("Cylinder").GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            /**
             * ...and also display his HP on the overlay UI
             */
            enemyHpTopBarDisplay.setTrackedEnemy(enemy);
        } else {
            /**
             * but reset the HP overlay UI if no enemy is pointed at currently
             */
            enemyHpTopBarDisplay.setTrackedEnemy();
        }
    }
    /**
     * are two points within a certain distance from each other if you ignore their height component?
     */
    bool nearEnough(Vector3 target, Vector3 currentPosition, float distance = 0.001f) {
        target.y = 0f;
        currentPosition.y = 0f;
        return Vector3.Distance(currentPosition, target) < distance;
    }
}
