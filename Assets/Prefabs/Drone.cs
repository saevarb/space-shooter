
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

    public float orbitDistance = 2f;
    public float orbitSpeed = 2f;
    public float maxSpeed = 4f;
    public float turningSpeed = 4f;


    private float orbitAngle;
    public State state = State.Idle;
    private Rigidbody2D body;
    private GameObject playerShip;
    private GameObject target;
    public Vector3 heading;
    private List<Vector3> path;

    public GameObject currentTarget {
        get {
            return target;
        }

        set {
            target = value;
        }
    }


    // Use this for initialization
    private void Awake() {
        Debug.Log($"Drone starting {state}");
        playerShip = GameObject.Find("mainPlayer");
        // lineRenderer.startWidth = 0.1f;
        // lineRenderer.endWidth = 0.1f;
        weapon = gameObject.AddComponent<Laser>();
        body =  GetComponent<Rigidbody2D>();
        // The drone starts pointing upwards
        heading = new Vector2(0, 1);
        transform.position = playerShip.GetComponent<Rigidbody2D>().position + Random.insideUnitCircle * orbitDistance;
    }
    void Start () {
        currentTarget = playerShip;
        state = State.Idle;
    }

    // Update is called once per frame
    void Update () {
        Debug.DrawRay(transform.position, 0.5f * heading.normalized, Color.red);
        if(currentTarget == null)
        {
            MovetoTarget(playerShip);
        }

        State oldState = state;
        if (!InRangeOfTarget()) {
            state = State.MovingToTarget;
        }
        switch(state) {
            case State.Idle: {
                break;
            }
            case State.MiningAsteroid: {
                weapon.FireWeapon(currentTarget);
                break;
            }

            case State.MovingToTarget: {
                if (InRangeOfTarget()) {
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

    private bool InRangeOfTarget() {
        if (currentTarget == null) return false;

        var heading = currentTarget.transform.position - transform.position;
        return heading.sqrMagnitude <= orbitDistance * orbitDistance;
    }

    public void MovetoTarget(GameObject obj)
    {
        currentTarget = obj;
        state = State.MovingToTarget;
        Pathfinder p = GetComponent<Pathfinder>();
        path = p.FindPath(obj.transform.position);
        weapon.StopFiring();
    }

    void Idle() {
        var pPos = playerShip.transform.position;
        OrbitPosition(pPos);
    }

    private void OrbitPosition(Vector3 pos) {
        var heading = pos - transform.position;
        if(InRangeOfTarget()) {
            var newHeading = new Vector3(heading.y, -heading.x).normalized;
            MoveToPoint(newHeading);
        } else {
            MoveToPoint(pos);
        }
    }

    void FixedUpdate() {
        if (currentTarget == null) {
            MovetoTarget(playerShip);
        }

        switch (state) {
            case State.Idle:
                Idle();
                break;
            case State.MovingToTarget:
                MoveToTarget();
                break;
            case State.MiningAsteroid:
                OrbitPosition(currentTarget.transform.position);
                break;
            default:
                Idle();
                break;
        }
    }

    private void MoveToTarget() {
        if (currentTarget == null) {
            currentTarget = playerShip;
        }
        var targetDirection = currentTarget.transform.position - transform.position;
        MoveToPoint(targetDirection);

    }

    private void MoveToPoint(Vector3 targetDirection) {
        var newHeading = Vector3.RotateTowards(heading, targetDirection, turningSpeed * Time.fixedDeltaTime, 0);
        heading = newHeading;
        body.AddForce(new Vector2(heading.x, heading.y).normalized, ForceMode2D.Impulse);
        body.velocity = body.velocity.normalized / (body.velocity.magnitude / maxSpeed);
    }
}

