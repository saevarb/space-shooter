
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {
    public enum State {
        Idle,
        MiningAsteroid,
        AttackingTarget,
        MovingToTarget,
        BackToShip
    }

    public Weapon weapon;
    public PhysicsMover mover;
    public Navigator navigator;
    public State state = State.Idle;
    private GameObject playerShip;

    public GameObject currentTarget { get; set; }


    // Use this for initialization
    private void Awake() {
        Debug.Log($"Drone starting {state}");
        playerShip = GameObject.Find("mainPlayer");
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        transform.position = playerShip.GetComponent<Rigidbody2D>().position + Random.insideUnitCircle * navigator.orbitDistance;
    }
    void Start () {
        currentTarget = playerShip;
        state = State.Idle;
    }

    // Update is called once per frame
    void Update () {
        if(currentTarget == null)
        {
            MoveToTarget(playerShip);
        }

        State oldState = state;
        switch(state) {
            case State.Idle: {
                if(!navigator.HasArrived()) {
                    navigator.SetDestination(playerShip.transform);
                }
                break;
            }
            case State.MiningAsteroid: {
                weapon.FireWeapon(currentTarget);
                break;
            }

            case State.MovingToTarget: {
                if (navigator.isActive && navigator.HasArrived()) {
                    Debug.Log($"In range of target {currentTarget.tag}");
                    switch (currentTarget.tag) {
                        case "Player": {
                            state = State.Idle;
                            break;
                        }
                        case "Mineable": {
                            state = State.MiningAsteroid;
                            break;
                        }
                        case "Enemy": {
                            state = State.AttackingTarget;
                            break;
                        }
                    }
                } 
                break;
            }
            default:
                break;
        }
        if(oldState != state) {
            Debug.Log($"Switched from {oldState} to {state}");
        }
    }


    public void MoveToTarget(GameObject obj)
    {
        if(currentTarget != obj) {
            currentTarget = obj;
            state = State.MovingToTarget;
            navigator.SetDestination(currentTarget);
        } else {
            if (!navigator.isActive)
                navigator.SetDestination(currentTarget);
        }
        weapon.StopFiring();
    }

    void Idle() {
        var pPos = playerShip.transform.position;
        navigator.OrbitPosition(pPos);
    }


    void FixedUpdate() {
        if (currentTarget == null) {
            MoveToTarget(playerShip);
        }

        switch (state) {
            case State.Idle:
                Idle();
                break;
            case State.MovingToTarget:
                MoveToTarget();
                break;
            case State.MiningAsteroid:
                navigator.OrbitPosition(currentTarget);
                break;
            default:
                Idle();
                break;
        }
    }

    private void MoveToTarget() {
        if (currentTarget == null) {
            //currentTarget = playerShip;
            return;
        }
        MoveToTarget(currentTarget);
    }

}

