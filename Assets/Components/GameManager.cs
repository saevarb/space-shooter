using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {

    private Targetable curTarget;
    public Text hpText;
    public Text nameText;
    public GameObject noTargetPanel;
    public GameObject targetPanel;

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
    }
}
