using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {

    public Text hpText;
    public Text nameText;
    public GameObject noTargetPanel;
    public GameObject targetPanel;
    public GameObject dronePrefab;
    public MainPlayer player;

    public Targetable curTarget;

    private List<Drone> drones;

    private bool oneClick = false;
    private float lastClickTime;
    private delegate void ResetShaderDelegate();
    private ResetShaderDelegate resetShaderDelegate;


    public GameManager() { }

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
        if(Input.GetMouseButtonDown(0)) {
            if(!oneClick) {
                lastClickTime = Time.time;
                oneClick = true;
            } else {
                if (Time.time - lastClickTime <= 0.5) {
                    Debug.LogWarning("Double click");
                    var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    player.MoveToPoint(pos);
                }
                oneClick = false;
            }
        }
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
            Debug.Log("attacking target");
            foreach(Drone d in drones) {
                if(curTarget != null)
                    d.MoveToTarget(curTarget.gameObject);
            }
        }
    }

    public void SetTarget(Targetable obj) {
        Debug.Log("Setting target");

        if(curTarget != null) {
            resetShaderDelegate();
            resetShaderDelegate = null;
        }
        curTarget = obj;
        var selectionShader = Shader.Find("selectionShader");
        curTarget.GetComponentsInChildren<Renderer>().ToList().ForEach(x => {
                var oldShader = x.material.shader;
                resetShaderDelegate += () => x.material.shader = oldShader;
                x.material.shader = selectionShader;
            });

    }

}
