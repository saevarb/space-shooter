using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour {
    private Navigator nav;

    private float maxSpeed = 10f;

    void Start () {
        nav = GetComponent<Navigator>();
    }

    void Update (){
    }

    void FixedUpdate() {
    }

    public void MoveToPoint(Vector3 point) {
        nav.SetDestination(point, 1);
    }

    public void MoveToTarget(Transform target) {
        nav.SetDestination(target, 1);
    }


}
