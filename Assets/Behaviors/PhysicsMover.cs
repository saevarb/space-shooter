using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMover : MonoBehaviour {

    [Range(1, 5)]
    public float orbitDistance = 2f;
    [Range(.1f, 20)]
    public float orbitSpeed = 2f;
    [Range(1, 10)]
    public float maxSpeed = 4f;
    [Range(1, 10)]
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
    }

	// Use this for initialization
	void Start () {
        heading = new Vector2(0, 1);
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(debug) {
            Debug.DrawRay(transform.position, 0.5f * heading.normalized, Color.red);
        }
		
	}
}
