using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IKillable {
    public GameObject playerShip;
    float hp = 10;

    // Use this for initialization
    void Start () {
        playerShip = GameObject.Find("mainPlayer");
    }

    // Update is called once per frame
    void Update () {
    }

    void OnMouseDown() {
        var mp = playerShip.GetComponent<MainPlayer>();
        mp.SetTarget(gameObject);
    }

    public void ApplyDamage(float dmg)
    {
        // Canvas dmgCanvas = GameObject.Find("dmgCanvas").GetComponent<Canvas>();

        hp -= dmg;
        if(hp <= 0) Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
