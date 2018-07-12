using MoreLinq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Debugger))]
public class Navigator : MonoBehaviour {

    enum NavState {
        MovingToPoint,
        MovingToTarget,
        Orbiting
    }

    public Pathfinder pathfinder;
    public PhysicsMover mover;
    [Range(0.5f, 10f)]
    public float pathRecalculateTimer = 1f;

    private Transform destination;
    private Vector3 targetPoint;
    private Vector3 curNode;
    private Queue<Vector3> curPath;
    private float updateTimer;
    private Debugger debugger;
    private NavState navState;
    private float arrivalDistance;

    private bool isActive_;

    public bool isActive {
        get {
            return isActive_;
        }
    }

    public bool hasArrived {
        get {
            switch (navState) {
                case NavState.MovingToPoint:
                    return CloseTo(targetPoint);
                case NavState.MovingToTarget:
                    return destination == null ? false : CloseTo(destination.position);
                default:
                    return false;
            }
        }
    }

    private bool CloseTo(Vector3 pos) {
        var heading = pos - transform.position;
        return heading.sqrMagnitude <= arrivalDistance * arrivalDistance;
    }

    public void OrbitPosition(GameObject target, float orbitDistance) {
        OrbitPosition(target.transform.position, orbitDistance);
    }

    public void OrbitPosition(Vector3 pos, float orbitDistance) {
        var heading = pos - transform.position;
        List<Vector3> orbitPoints = new List<Vector3>();
        for (float angle = 0; angle <= Mathf.PI * 2; angle += 2 * Mathf.PI / 16) {
            Vector3 v = orbitDistance * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            //Debug.DrawRay(cur, v, Color.green);
            orbitPoints.Add(pos + v);
            debugger.DrawPoint(pos + v, Color.yellow);
        }
        Vector3 closest = orbitPoints.MinBy(x => Vector3.SqrMagnitude(pos - x)).First();
        int pathStart = orbitPoints.FindIndex(x => x == closest);
        var pathInit = orbitPoints.Skip(pathStart).ToList();
        var pathTail = orbitPoints.Take(pathStart).ToList();
        pathInit.AddRange(pathTail);
        curPath = new Queue<Vector3>(pathInit);
        curNode = curPath.Dequeue();
        targetPoint = pathInit.Last();
        navState = NavState.Orbiting;
        arrivalDistance = 0.5f * Vector3.Distance(orbitPoints[0], orbitPoints[1]);
        isActive_ = true;
    }

    public void SetDestination(Transform target, float arrDist) {
        curPath = new Queue<Vector3>(pathfinder.FindPath(target.position));
        if (curPath == null)
            return;
        destination = target;
        navState = NavState.MovingToTarget;
        curNode = curPath.Dequeue();
        isActive_ = true;
        arrivalDistance = arrDist;
    }

    public void SetDestination(GameObject target, float arrivalDistance) {
        SetDestination(target.transform, arrivalDistance);
    }

    public void SetDestination(Vector3 target, float arrDist) {
        navState = NavState.MovingToPoint;
        curPath = new Queue<Vector3>(pathfinder.FindPath(target));
        if (curPath == null)
            return;
        isActive_ = true;
        curNode = curPath.Dequeue();
        targetPoint = target;
        arrivalDistance = arrDist;
    }

    private void ResetState() {
        curPath = null;
        isActive_ = false;
    }

    void Start () {
        debugger = GetComponent<Debugger>();
	}

    private void Update() {
        updateTimer += Time.deltaTime;

        var pathAsList = curPath == null ? null : curPath.ToList();
        if (pathAsList == null)
            return;
        for (int i = 1; i < curPath.Count; i++) {
            debugger.DrawLine(pathAsList[i - 1], pathAsList[i], Color.cyan);
        }
        //if(updateTimer >= pathRecalculateTimer) {
        //    updateTimer = 0;
        //    if(isActive) {
        //        SetDestination(destination);
        //    }
        //}
    }

    void FixedUpdate () {
        if (!isActive || curPath == null)
            return;
        Debug.Assert(curPath != null);
        switch (navState) {
            case NavState.MovingToPoint:
            case NavState.MovingToTarget: {
                if(hasArrived) {
                    mover.Break();
                    ResetState();
                    return;
                }
                if (CloseTo(curNode) && curPath.Count != 0) {
                    curNode = curPath.Dequeue();
                } else {
                    mover.MoveToPoint(curNode);
                }
                break;
            }
            case NavState.Orbiting: {
                if(CloseTo(curNode)) {
                    curPath.Enqueue(curNode);
                    curNode = curPath.Dequeue();
                } else {
                    mover.MoveToPoint(curNode);
                }
                break;
            }
            default:
                break;
        }
         //if (!isActive || curPath == null)
         //    return;
         //if(!HasArrived() && curPath.Count == 0) {
         //    switch(navState) {
         //        case NavState.MovingToPoint:
         //            SetDestination(targetPoint, arrivalDistance);
         //            break;
         //        case NavState.MovingToTarget:
         //            SetDestination(destination, arrivalDistance);
         //            break;
         //    }
         //    return;
         //} else if (isActive && curPath.Count > 0) {
         //    if(CloseTo(curNode)) {
         //        curNode = curPath.Dequeue();
         //    } else {
         //        mover.MoveToPoint(curNode);
         //    }
         //} else if (HasArrived() && curPath.Count == 0) {
         //    mover.Break();
         //    ResetState();
         //}
    }

}
