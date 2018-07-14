using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour {

    private Dictionary<Tuple<Vector3, Vector3>, Color> lines;
    private Dictionary<Vector3, Color> dots;
    private SpriteRenderer debugPoint;
    private Dictionary<Vector3, SpriteRenderer> dotObjects;
	// Use this for initialization
    public void DrawLine(Vector3 start, Vector3 end, Color color) {
        var line = new Tuple<Vector3, Vector3>(start, end);
        if(!lines.ContainsKey(line))
            lines.Add(line, color);
    }
    public void DrawLines(IEnumerable<Tuple<Vector3, Vector3>> lines, Color color) {
        foreach(var line in lines) {
            DrawLine(line.Item1, line.Item2, color);
        }
    }
    public void DrawPoint(Vector3 p, Color color) {
        if(!dots.ContainsKey(p)) {
            dots.Add(p, color);
            //SpriteRenderer dot = Instantiate<SpriteRenderer>(debugPoint, p, Quaternion.identity);
            //dot.color = color;
            //dotObjects.Add(p, dot);
        } else {
            dots[p] = color;
        }
    }
    public void DrawPoints(IEnumerable<Vector3> ps, Color color) {
        foreach(var p in ps) {
            DrawPoint(p, color);
        }
    }
    public void Clear() {
        foreach(var kv in dotObjects) {
            Destroy(kv.Value.gameObject);
        }
        lines.Clear();
        dots.Clear();
        dotObjects.Clear();
    }
    private void Awake() {
        lines = new Dictionary<Tuple<Vector3, Vector3>, Color>();
        dots = new Dictionary<Vector3, Color>();
        debugPoint = (Resources.Load("debugPoint", typeof(GameObject)) as GameObject).GetComponent<SpriteRenderer>();
        dotObjects = new Dictionary<Vector3, SpriteRenderer>();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach(var kv in lines) {
            var line = kv.Key;
            var color = kv.Value;
            Debug.DrawLine(line.Item1, line.Item2, color);
        }
        foreach (var kv in dots) {
            var p = kv.Key;
            var color = kv.Value;
            float offset = 0.08f;
            var topLeft = offset * (Vector3.left + Vector3.up) + p;
            var bottomLeft = offset * (Vector3.left + Vector3.down) + p;
            var topRight = offset * (Vector3.right + Vector3.up) + p;
            var bottomRight = offset * (Vector3.right + Vector3.down) + p;
            Debug.DrawLine(topLeft, topRight, color);
            Debug.DrawLine(topRight, bottomRight, color);
            Debug.DrawLine(bottomRight, bottomLeft, color);
            Debug.DrawLine(bottomLeft, topLeft, color);
        }
	}
}
