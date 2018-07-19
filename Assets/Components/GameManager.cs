using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class GameManager : MonoSingleton<GameManager> {

    public Text hpText;
    public Text nameText;
    public GameObject noTargetPanel;
    public GameObject targetPanel;
    public GameObject dronePrefab;
    public MainPlayer player;
    public float targetCircleRadius;

    private Targetable curTarget;
    private LineRenderer targetCircle;
    private List<Drone> drones;

    private bool oneClick = false;
    private float lastClickTime;

    public GameManager() { }

    void Start() {
        Debug.Log("Game manager starting ..");

        drones = new List<Drone>();

        targetCircle = GetComponent<LineRenderer>();
        targetCircle.sortingLayerName = "Default";
        targetCircle.material = new Material(Shader.Find("Sprites/Default"));
        targetCircle.startColor = Color.cyan;
        targetCircle.endColor = Color.cyan;
        targetCircle.startWidth = .05f;
        targetCircle.endWidth = .05f;


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
            targetCircle.enabled = false;
        } else {
            int vertexCount = 32;
            var pos = curTarget.transform.position;
            var circlePoints = new List<Vector3>();
            var collider = curTarget.GetComponent<Collider2D>();
            if(collider) {
                targetCircleRadius = collider.bounds.size.magnitude;
            }
            for (float angle = 0; angle <= Mathf.PI * 2; angle += 2 * Mathf.PI / vertexCount) {
                Vector3 v = targetCircleRadius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                circlePoints.Add(pos + v);
            }
            circlePoints.Add(circlePoints[0]);

            targetCircle.positionCount = vertexCount + 1;
            targetCircle.SetPositions(circlePoints.ToArray());
            targetCircle.enabled = true;

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

        curTarget = obj;
        obj.GetComponent<MeshRenderer>().material.shader = Shader.Find("Shaders/selectionShader");

    }

}
