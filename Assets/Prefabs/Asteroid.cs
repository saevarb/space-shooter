using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HasHealth))]
public class Asteroid : MonoBehaviour {
    public ParticleSystem explosionPrefab;

    private void OnDestroy() {
        Instantiate(explosionPrefab, this.transform);
    }
}
