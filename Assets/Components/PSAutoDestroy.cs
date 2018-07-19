using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSAutoDestroy : MonoBehaviour {

    private ParticleSystem ps;
    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        if(ps == null) {
            Debug.LogError($"PSAutoDestroy attached to object {this} with no particle system");
        } else {
            Debug.Log($"Destroying explosion after {ps.main.duration}");
            Destroy(gameObject, ps.main.duration);
        }
    }
    // Update is called once per frame
    void Update () {
    }
}
