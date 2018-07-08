using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    public GameObject playerShip;

    // Use this for initialization
    void Start () {
        playerShip = GameObject.Find("playerShip");
    }

    // Update is called once per frame
    void Update () {

    }

    void OnMouseDown() {
        var mp = playerShip.GetComponent<MainPlayer>();

        mp.SetTarget(gameObject);
    }
}
