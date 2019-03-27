using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpTopbarDisplay : MonoBehaviour
{
    /**
     * the enemy that we are tracking the HP of
     */
    protected GameObject trackedEnemy;
    public Texture2D bgImage;
    public Texture2D fgImage;

    float healthBarLength = 100;

    void Start() {
        bgImage = Resources.Load("hp-background") as Texture2D;
        fgImage = Resources.Load("hp-foreground") as Texture2D;
    }

    public void setTrackedEnemy(GameObject value) {
        trackedEnemy = value;
    }
    public void setTrackedEnemy() {
        trackedEnemy = null;
    }

    void OnGUI() {
        Color bgColor = GUI.backgroundColor;
        bgColor.a = 0f;
        GUI.backgroundColor = bgColor;

        if (bgImage == null || fgImage == null || trackedEnemy == null) {
            return;
        }
        Health enemyHealth = trackedEnemy.GetComponent<Health>();
        GUI.BeginGroup (new Rect (Mathf.Floor(Screen.width / 2 - healthBarLength / 2), 15, healthBarLength,32));
        GUI.Box (new Rect (0,0, healthBarLength,32), bgImage, new GUIStyle());
        GUI.BeginGroup (new Rect (-4, -4, Mathf.Ceil(enemyHealth.curHealth / enemyHealth.maxHealth * healthBarLength), 32));
        GUI.Box (new Rect (0,0,healthBarLength,32), fgImage);
        GUI.EndGroup ();
        GUI.EndGroup ();
    }



}
