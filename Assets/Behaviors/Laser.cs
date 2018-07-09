using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon {
    protected new float weaponDuration = 0.75f;
    protected new float weaponCooldown = 0.3f;
    protected new float weaponDamage = 0.1f;

    private LineRenderer laserRenderer;

    void Start () {
        laserRenderer = gameObject.AddComponent<LineRenderer>();
        laserRenderer.startWidth = .03f;
        laserRenderer.endWidth = .03f;
        laserRenderer.enabled = true;
	}
	
	void Update () {
		
	}

    public override void FireWeapon(GameObject target) {
        laserRenderer.SetPositions(new Vector3[] { transform.position, target.transform.position });

        if (laserRenderer.enabled) { // if we are shooting
                                     // subtract time that has passed from the timer
            weaponTimer -= Time.deltaTime;

            if (weaponTimer <= 0) { // If the timer has expired, we are done shooting for now
                                   // so we turn the laser off
                laserRenderer.enabled = false;
                // and set the timer to the cooldown value
                weaponTimer = weaponCooldown;
            }
        } else { // if we aren't shooting, then we are cooling down
                 // subtract time that has passed from the timer
            weaponTimer -= Time.deltaTime;

            if (weaponTimer <= 0) { // If the timer has expired, the cooldown is over
                                   // so we turn the laser on
                laserRenderer.enabled = true;
                // and set the timer to the laser duration
                weaponTimer = weaponDuration;
                target.GetComponent<Killable>().ApplyDamage(weaponDamage);
            }
        }
    }
}
