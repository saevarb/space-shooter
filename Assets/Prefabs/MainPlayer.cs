using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour {
    public GameObject dronePrefab;

    private float speed = 10f;

    private List<Drone> drones;

    void Start () {
        drones = new List<Drone>();

        for(int i = 0; i < 5; i++) {
            GameObject drone = Instantiate(dronePrefab) as GameObject;
            var d = drone.GetComponent<Drone>();
            Debug.Log($"Instantiating drone");
            drones.Add(d);
        }
    }

    void Update (){
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(mouse, Vector3.back);
        var a = Input.GetKeyDown("i");
        if (a)
        {
            GameObject drone = Instantiate(dronePrefab) as GameObject;
            var d = drone.GetComponent<Drone>();
            drones.Add(d);
        }
    }

    void FixedUpdate() {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        body.AddForce(speed * new Vector2(x, y));
    }

    public void SetTarget(GameObject obj) {
        Debug.Log("Setting target");
        foreach(Drone d in drones) {
            d.MovetoTarget(obj);
        }
    }
}
