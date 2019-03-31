using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction
{
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

    public PlayerAction(GameObject moveTowardsGameObject){
        this.moveTowardsGameObject = moveTowardsGameObject;
    }
    public PlayerAction(Vector3 moveTowardsPointOnGround){
        this.moveTowardsPointOnGround = moveTowardsPointOnGround;
    }

    public bool isMovingTowardsPointOnGround() {
        return moveTowardsGameObject == null;
    }

    public Vector3 getPosition() {
        if (moveTowardsGameObject) {
            return moveTowardsGameObject.transform.position;
        } else {
            return moveTowardsPointOnGround;
        }
    }
}
