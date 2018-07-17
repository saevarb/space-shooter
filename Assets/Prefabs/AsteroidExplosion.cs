using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour {
    [Range(0.1f, 1f)]
    public float duration;
    private ParticleSystem ps;

    void Start () {
        ps = GetComponent<ParticleSystem>();
        ps.startLifetime = duration;
        ps.Play();
    }

    void Update() {
        if(!ps.IsAlive())
            Destroy(gameObject);
    }

}
