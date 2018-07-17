using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour {

    private float maxSpeed = 10f;

    void Start () {
    }

    void Update (){
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(mouse, Vector3.back);
    }

    void FixedUpdate() {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.AddForce(new Vector2(x, y).normalized);
        if(body.velocity != Vector2.zero && body.velocity.magnitude > maxSpeed)
            body.velocity = body.velocity.normalized / (body.velocity.magnitude / maxSpeed);
    }

 
}
