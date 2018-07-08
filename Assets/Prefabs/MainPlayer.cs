using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour {
    public GameObject dronePrefab;

    private float speed = 10f;

    private List<Drone> drones;

    void Start () {
        drones = new List<Drone>();
        var dronePos = new Vector3(2, 0, 0) + transform.position;
        GameObject drone = Instantiate(dronePrefab, dronePos, Quaternion.identity) as GameObject; 
        var d = drone.GetComponent<Drone>();
        Debug.Log($"Instantiating drone at {dronePos}");
        drones.Add(d);
    }

    void Update (){
       var a = Input.GetKeyDown("i");
        if (a)
        {
            var dronePos = new Vector3(2, 0, 0) + transform.position;
            GameObject drone = Instantiate(dronePrefab, dronePos, Quaternion.identity) as GameObject;
            var d = drone.GetComponent<Drone>();
            drones.Add(d);
        }
    }


    void FixedUpdate() {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        Debug.Log($"{x},{y}");

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
