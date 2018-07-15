using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour {

    public float weaponDuration;
    public float weaponCooldown;
    public float weaponDamage;
    public float weaponRange;

    protected float weaponTimer;
    protected WeaponState weaponState;
    protected Transform curTarget;

    abstract public void OnDoneFiring();
    abstract public void OnStartFiring();

    protected enum WeaponState {
        Firing,
        CoolingDown,
        Idle
    }

    public void StartFiring(GameObject target) {
        StartFiring(target.transform);
    }

    public void StartFiring(Transform target) {
        if (Vector3.Distance(transform.position, target.position) > weaponRange)
            return;
        weaponState = WeaponState.Firing;
        weaponTimer = weaponDuration;
        curTarget = target;
    }

    public void StopFiring() {
        weaponState = WeaponState.Idle;
        weaponTimer = weaponDuration;
        OnDoneFiring();
    }

	void Start () {
        weaponState = WeaponState.Idle;
        weaponTimer = weaponDuration;
    }
	
	void Update () {
        switch(weaponState) {
            case WeaponState.Idle:
                return;
            case WeaponState.Firing: {
                if (weaponTimer <= 0) { // If the timer has expired, we are done shooting for now
                    weaponTimer = weaponCooldown;
                    weaponState = WeaponState.CoolingDown;
                    curTarget.GetComponent<HasHealth>().ApplyDamage(weaponDamage);
                    OnDoneFiring();
                }
                break;
            }
            case WeaponState.CoolingDown: {
                if (weaponTimer <= 0) { // If the timer has expired, the cooldown is over
                    weaponTimer = weaponDuration;
                    weaponState = WeaponState.Firing;
                    OnStartFiring();
                }
                break;
            }
        }

        // subtract time that has passed from the timer
        weaponTimer -= Time.deltaTime;
    }
}
