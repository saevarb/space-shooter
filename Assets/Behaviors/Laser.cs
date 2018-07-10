using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon {

    private LineRenderer laserRenderer;

    void Awake() {
        laserRenderer = gameObject.AddComponent<LineRenderer>();
        laserRenderer.startWidth = .03f;
        laserRenderer.endWidth = .03f;
        laserRenderer.material = Resources.Load("laserMaterial", typeof(Material)) as Material;
        weaponFiring = false;
        weaponTimer = 0;
        weaponDuration = 0.75f;
        weaponCooldown = 0.3f;
        weaponDamage = 0.1f;
    }

    void Start() {
    }

    void Update() {

    }

    public override void FireWeapon(GameObject target) {
        laserRenderer.SetPositions(new Vector3[] { transform.position, target.transform.position });

        // subtract time that has passed from the timer
        weaponTimer -= Time.deltaTime;
        if (weaponFiring) { // if we are shooting
            laserRenderer.enabled = true;

            if (weaponTimer <= 0) { // If the timer has expired, we are done shooting for now
                target.GetComponent<Killable>().ApplyDamage(weaponDamage);
                // so we turn the laser off
                weaponFiring = false;
                // and set the timer to the cooldown value
                weaponTimer = weaponCooldown;
            }
        } else { // if we aren't shooting, then we are cooling down
            laserRenderer.enabled = false;

            if (weaponTimer <= 0) { // If the timer has expired, the cooldown is over

                // so we turn the laser on
                weaponFiring = true;
                // and set the timer to the laser duration
                weaponTimer = weaponDuration;
            }
        }
    }

    public override void StopFiring() {
        laserRenderer.enabled = false;
        weaponFiring = false;
        weaponTimer = 0;
    }

}
