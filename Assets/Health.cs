using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float curHealth = 90;

    protected bool dead = false;

    public float damageFor(float amount) {
        if (amount < 0) {
            throw new System.ArgumentException(System.String.Format("cannot take negative damage ({0})", amount), "amount");
        }
        curHealth -= amount;
        if (curHealth <= 0) {
            dead = true;
            curHealth = 0;
        }
        return curHealth;
    }

    public bool isDead() {
        return dead;
    }
}
