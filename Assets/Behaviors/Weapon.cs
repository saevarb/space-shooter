using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour {

    protected float weaponDuration;
    protected float weaponCooldown;
    protected float weaponDamage;
    protected float weaponTimer;
    protected bool weaponFiring;
    protected bool weaponEnabled;
	// Use this for initialization
    abstract public void FireWeapon(GameObject target);
    abstract public void StopFiring();

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
