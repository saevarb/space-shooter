﻿using System;
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
        var offset = UnityEngine.Random.insideUnitCircle * 1.5f;
        transform.position = playerShip.GetComponent<Rigidbody>().position + new Vector3(offset.x, offset.y, 0);
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
                if (!navigator.isActive) {
                    Idle();
                } else if(!navigator.hasArrived) {
                    MoveToTarget(playerShip);
                }
                break;
            }
            case State.MovingToTarget: {
                if (navigator.isActive && navigator.hasArrived) {
                    Debug.Log($"In range of target {currentTarget.tag}");
                    switch (currentTarget.tag) {
                        case "Player": {
                            Idle();
                            break;
                        }
                        case "Mineable": {
                            MineAsteroid();
                            break;
                        }
                        case "Enemy": {
                            AttackTarget();
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

    private void MineAsteroid() {
        state = State.MiningAsteroid;
        navigator.Follow(currentTarget, weapon.weaponRange);
        weapon.StartFiring(currentTarget);
    }

    private void AttackTarget() {
        state = State.AttackingTarget;
        navigator.Follow(currentTarget, weapon.weaponRange);
        weapon.StartFiring(currentTarget);
    }

    public void MoveToTarget(GameObject obj)
    {
        if(currentTarget != obj) {
            currentTarget = obj;
            state = State.MovingToTarget;
            navigator.SetDestination(currentTarget, weapon.weaponRange);
        } else {
            if (!navigator.isActive)
                navigator.SetDestination(currentTarget, weapon.weaponRange);
        }
        weapon.StopFiring();
    }

    void Idle() {
        state = State.Idle;
        navigator.Follow(playerShip, 1f);
        weapon.StopFiring();
    }


    void FixedUpdate() {
    }

    private void MoveToTarget() {
        if (currentTarget == null) {
            //currentTarget = playerShip;
            return;
        }
        MoveToTarget(currentTarget);
    }

}

