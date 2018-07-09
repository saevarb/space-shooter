using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour {
    private GameObject playerShip;

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

}
