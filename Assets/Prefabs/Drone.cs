using System;
using UnityEngine;

enum State {
    Idling,
    MiningAsteroid,
    AttackingTarget,
    MovingToTarget,
    BackToShip
}

public class Drone : MonoBehaviour {
    public GameObject playerShip;

    private GameObject target;

    private float orbitAngle;
    private float orbitDistance = 2f;
    private float orbitSpeed = 2f;
    private float speed = 2f;
    private State state = State.Idling;

    // Use this for initialization
    void Start () {
        Debug.Log($"Drone starting {state}");
    }

    // Update is called once per frame
    void Update () {
        orbitAngle += orbitSpeed * Time.deltaTime;
        switch(state) {
            case State.Idling: {
                break;
            }
            case State.MovingToTarget: {
                var heading = target.transform.position - transform.position;
                if (heading.sqrMagnitude <= orbitDistance * orbitDistance) {
                    state = State.MiningAsteroid;
                }
                break;
            }
            default:
                break;
        }
    }

    public void MovetoTarget(GameObject obj)
    {
        target = obj;
        state = State.MovingToTarget;
    }

    void Idle() {
        var pPos = playerShip.transform.position;
        OrbitPosition(pPos);
    }

    private void OrbitPosition(Vector3 pos) {
        transform.position = pos + orbitDistance * new Vector3(Mathf.Cos(orbitAngle), Mathf.Sin(orbitAngle), 0);
    }

    void FixedUpdate() {
        switch(state) {
            case State.Idling:
                Idle();
                break;
            case State.MovingToTarget:
                MoveToTarget();
                break;
            case State.MiningAsteroid:
                OrbitPosition(target.transform.position);
                break;
            default:
                Idle();
                break;
        }
    }

    private void MoveToTarget() {
        if (target == null) {
            target = playerShip;
        }
        var tpos = target.transform.position;
        var heading = tpos - transform.position;

        transform.position += heading.normalized * speed * Time.deltaTime; 
    }
}
