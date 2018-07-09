
using UnityEngine;

enum State {
    Idle,
    MiningAsteroid,
    AttackingTarget,
    MovingToTarget,
    BackToShip
}

public class Drone : MonoBehaviour {
    private GameObject playerShip;

    private GameObject target;

    public float orbitDistance = 2f;
    public float orbitSpeed = 2f;
    public float speed = 2f;

    private float orbitAngle;
    private State state = State.Idle;
    public Weapon weapon;

    // Use this for initialization
    void Start () {
        Debug.Log($"Drone starting {state}");
        playerShip = GameObject.Find("mainPlayer");
        // lineRenderer.startWidth = 0.1f;
        // lineRenderer.endWidth = 0.1f;
        weapon = gameObject.AddComponent<Laser>();
    }

    // Update is called once per frame
    void Update () {
        orbitAngle += orbitSpeed * Time.deltaTime;

        if(target == null)
        {
            MovetoTarget(playerShip);
        }
        switch(state) {
            case State.Idle: {
                break;
            }
            case State.MiningAsteroid: {
                weapon.FireWeapon(target);
                break;
            }

            case State.MovingToTarget: {
                var heading = target.transform.position - transform.position;
                if (heading.sqrMagnitude <= orbitDistance * orbitDistance) {
                    switch(target.tag) {
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
        transform.position = pos + orbitDistance * new Vector3(Mathf.Cos(orbitAngle), Mathf.Sin(orbitAngle), 0);
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
        var tpos = target.transform.position;
        var heading = tpos - transform.position;

        transform.position += heading.normalized * speed * Time.deltaTime; 
    }
}
