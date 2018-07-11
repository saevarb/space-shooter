using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon {

    //public new float weaponDuration = 0.75f;
    //public new float weaponCooldown = 0.3f;
    //public new float weaponDamage = 0.1f;
    public GameObject laserPrefab;
    private SpriteRenderer laser;

    public override void OnDoneFiring() {
        laser.enabled = false;
    }

    public override void OnStartFiring() {
        var heading = curTarget.position - transform.position;
        var distance = Vector3.Magnitude(heading);
        var scaleFactor = distance / laser.bounds.size.y;
        laser.transform.localScale = new Vector3(transform.localScale.x, scaleFactor, transform.localScale.y);
        //laser.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (heading.y / heading.x) + 0);
        laser.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * (heading.y / heading.x) - 90);
        //laser.transform.position = transform.position;
        laser.enabled = true;
    }

    void Awake() {
        laser = (Instantiate(laserPrefab) as GameObject).GetComponent<SpriteRenderer>();
        laser.transform.parent = transform;

        weaponTimer = 0;
    }

    void Start() {
    }

    //public override void StartFiring(Transform target) {
    //    isFiring = true;
    //    curTarget = target;
    //}

    //public override void StopFiring() {
    //    laserRenderer.enabled = false;
    //    isFiring = false;
    //    weaponTimer = 0;
    //}

}
