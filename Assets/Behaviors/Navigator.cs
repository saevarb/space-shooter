using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour {

    public Pathfinder pathfinder;
    public PhysicsMover mover;
    public float arrivalDistance = 2f;
    public float orbitDistance = 2f;
    public float pathRecalculateTimer = 1f;

    private Transform destination;
    private Vector3 curNode;
    private List<Vector3> curPath;
    private float updateTimer;

    private bool isActive_;

    public bool isActive {
        get {
            return isActive_;
        }

    }

    public bool HasArrived() {
        if (destination == null) return false;
        return CloseTo(destination.position);
    }

    private bool CloseTo(Vector3 pos) {
        var heading = pos - transform.position;
        return heading.sqrMagnitude <= arrivalDistance * arrivalDistance;
    }

    public void OrbitPosition(GameObject target) {
        OrbitPosition(target.transform.position);
    }

    public void OrbitPosition(Vector3 pos) {
        var heading = pos - transform.position;
        //if(HasArrived()) {
        //    var newHeading = new Vector3(heading.y, -heading.x).normalized;
        //    MoveToPoint(newHeading);
        //} else {
        //    MoveToPoint(pos);
        //}
    }

    public void SetDestination(Transform target) {
        destination = target;
        curPath = pathfinder.FindPath(destination.position);
        if (curPath == null)
            return;
        curNode = curPath[0];
        curPath.RemoveAt(0);
        isActive_ = true;
    }

    public void SetDestination(GameObject target) {
        SetDestination(target.transform);
    }

    private void ResetState() {
        curPath = null;
        isActive_ = false;
    }

    void Start () {
	}

    private void Update() {
        updateTimer += Time.deltaTime;

        if(updateTimer >= pathRecalculateTimer) {
            updateTimer = 0;
            if(isActive) {
                SetDestination(destination);
            }
        }
    }

    void FixedUpdate () {
        if (!isActive || curPath == null)
            return;
        if(!HasArrived() && curPath.Count == 0) {
            SetDestination(destination);
            return;
        } else if (!HasArrived() && curPath.Count > 0) {
            if(CloseTo(curNode)) {
                curNode = curPath[0];
                curPath.RemoveAt(0);
            } else {
                mover.MoveToPoint(curNode);
            }
        } else if (HasArrived()) {
            ResetState();
        }
	}

}
