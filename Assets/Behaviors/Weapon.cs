using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour {

    protected float weaponDuration;
    protected float weaponCooldown;
    protected float weaponDamage;
    protected float weaponTimer;
	// Use this for initialization
	void Start () {
		
	}
    abstract public void FireWeapon(GameObject target);
	
	// Update is called once per frame
	void Update () {
		
	}
}
