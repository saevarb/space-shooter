using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour {
    [Range(0.1f, 1f)]
    public float duration;
    public ParticleSystem explosionPrefab;

    void Awake() {
    }

    void Start () {
    }

    void Update() {
        // Debug.Log("updating ps");
        // if(!ps.IsAlive())
        //     Destroy(gameObject);
    }


    public void OnDestroy() {
        Debug.LogWarning("Instantiating explosion");
        var ps = Instantiate(explosionPrefab);
        ps.transform.position = transform.position;
        ps.Play();

    }

}
