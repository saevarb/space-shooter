using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {

    public Text hpText;
    public Text nameText;
    public GameObject noTargetPanel;
    public GameObject targetPanel;
    public GameObject dronePrefab;

    private Targetable curTarget;
    private List<Drone> drones;

    public GameManager() { }

    public void SetTarget(Targetable obj) {
        Debug.Log("Setting target");

        curTarget = obj;
        //foreach (Drone d in drones) {
        //    d.MoveToTarget(obj);
        //}
    }

    void Start() {
        Debug.Log("Game manager starting ..");
        drones = new List<Drone>();

        for(int i = 0; i < 1; i++) {
            GameObject drone = Instantiate(dronePrefab) as GameObject;
            var d = drone.GetComponent<Drone>();
            Debug.Log($"Instantiating drone");
            drones.Add(d);
        }
    }

    void Update() {
        if (curTarget == null) {
            noTargetPanel.SetActive(true);
            targetPanel.SetActive(false);
        } else {
            noTargetPanel.SetActive(false);
            targetPanel.SetActive(true);
            hpText.text = curTarget.GetComponent<HasHealth>().health.ToString();
            nameText.text = curTarget.name;
        }

        var a = Input.GetKeyDown("i");
        if (a)
        {
            GameObject drone = Instantiate(dronePrefab) as GameObject;
            var d = drone.GetComponent<Drone>();
            drones.Add(d);
        }

        if(Input.GetKey("a")) {
            foreach(Drone d in drones) {
                if(curTarget != null)
                    d.MoveToTarget(curTarget.gameObject);
            }
        }
    }
}
