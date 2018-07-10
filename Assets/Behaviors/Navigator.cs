using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour {

    public Pathfinder pathfinder;
    public PhysicsMover mover;
    public float arrivalDistance = 2f;
    public float orbitDistance = 2f;

    private Vector3 destination;
    private Vector3 curNode;
    private List<Vector3> curPath;

    private bool isActive_;

    public bool isActive {
        get {
            return isActive_;
        }

    }

    public bool HasArrived() {
        if (!isActive_) return false;
        return CloseTo(destination);
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

    public void SetDestination(Vector3 target) {
        destination = target;
        curPath = pathfinder.FindPath(destination);
        if (curPath == null)
            return;
        curNode = curPath[0];
        curPath.RemoveAt(0);
        isActive_ = true;
    }

    public void SetDestination(GameObject target) {
        SetDestination(target.transform.position);
    }

    private void ResetState() {
        curPath = null;
        isActive_ = false;
    }

    void Start () {
		
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
