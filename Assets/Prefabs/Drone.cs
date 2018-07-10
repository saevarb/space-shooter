
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
        target = playerShip;
        state = State.Idle;
    }

    // Update is called once per frame
    void Update () {

        Debug.DrawRay(transform.position, 0.5f * heading.normalized, Color.red);
        if(target == null)
        {
            MovetoTarget(playerShip);
        }

        State oldState = state;
        switch(state) {
            case State.Idle: {
                if (!InRangeOfTarget()) {
                    state = State.MovingToTarget;
                }
                break;
            }
            case State.MiningAsteroid: {
                weapon.FireWeapon(target);
                break;
            }

            case State.MovingToTarget: {
                if (InRangeOfTarget()) {
                    Debug.Log($"In range of target {target.tag}");
                    switch (target.tag) {
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
                } else {
                    Debug.Log("Not in range of target");
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
        if (target == null) return false;

        var heading = target.transform.position - transform.position;
        return heading.sqrMagnitude <= orbitDistance * orbitDistance;
    }

    public void MovetoTarget(GameObject obj)
    {
        target = obj;
        state = State.MovingToTarget;
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
            Debug.DrawRay(transform.position, newHeading);
            MoveToPoint(newHeading);
        } else {
            MoveToPoint(pos);
        }
    }

    void FixedUpdate() {
        if (target == null) {
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
        var targetDirection = target.transform.position - transform.position;
        MoveToPoint(targetDirection);

    }

    private void MoveToPoint(Vector3 targetDirection) {
        var newHeading = Vector3.RotateTowards(heading, targetDirection, turningSpeed * Time.fixedDeltaTime, 0);
        heading = newHeading;
        body.AddForce(new Vector2(heading.x, heading.y).normalized, ForceMode2D.Impulse);
        body.velocity = body.velocity.normalized / (body.velocity.magnitude / maxSpeed);
    }
}
