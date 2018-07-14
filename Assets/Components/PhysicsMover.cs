using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMover : MonoBehaviour {

    [Range(1, 10)]
    public float maxSpeed = 4f;
    [Range(1, 20)]
    public float turningSpeed = 4f;
    public bool debug = false;

    private Vector3 heading;
    private Rigidbody2D body;

    public void MoveToPoint(Vector3 dest) {
        var targetDirection = dest - transform.position;
        var newHeading = Vector3.RotateTowards(heading, targetDirection, turningSpeed * Time.fixedDeltaTime, 0);
        heading = newHeading;
        body.AddForce(new Vector2(heading.x, heading.y).normalized, ForceMode2D.Impulse);
        body.velocity = body.velocity.normalized / (body.velocity.magnitude / maxSpeed);
        AdjustRotation();
    }

    public void Break() {
        body.AddForce(body.velocity * -1 * 0.99f, ForceMode2D.Impulse);
        AdjustRotation();
    }

    // Use this for initialization
    void Start () {
        heading = new Vector2(0, 1);
        body = GetComponent<Rigidbody2D>();
	}

    private void AdjustRotation() {
        if (heading.x != 0f)
            body.rotation = heading.y / heading.x * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update () {
        if(debug) {
            Debug.DrawRay(transform.position, 0.5f * heading.normalized, Color.red);
        }
	}
    private void FixedUpdate() {
        AdjustRotation();
    }
}
