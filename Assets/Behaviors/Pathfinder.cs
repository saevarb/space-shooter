using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoreLinq;

public class Pathfinder : MonoBehaviour {

    HashSet<Vector3> closedSet;
    List<Vector3> openSet;
    Dictionary<Vector3, Vector3> cameFrom;
    Dictionary<Vector3, float> gScore;
    Dictionary<Vector3, float> fScore;
    private Debugger debugger;
    // Use this for initialization
    private List<Vector3> GenerateNeighbors(Vector3 cur) {
        List<Vector3> neighbors = new List<Vector3>();
        for(float angle = 0; angle <= Mathf.PI * 2; angle += 2 * Mathf.PI / 10) {
            Vector3 v = 0.8f * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            //Debug.DrawRay(cur, v, Color.green);
            neighbors.Add(cur + v);
        }
        return neighbors;
    }

    private float GetScore(Dictionary<Vector3, float> scoreTable, Vector3 key) {
        if(scoreTable.ContainsKey(key)) {
            return scoreTable[key];
        } else {
            return float.MaxValue/2;
        }
    }

    public List<Vector3> FindPath(Vector3 dest) {
        closedSet.Clear();
        openSet.Clear();
        cameFrom.Clear();
        gScore.Clear();
        fScore.Clear();
        var start = transform.position;

        openSet.Add(start);
        gScore.Add(start, 0);
        fScore.Add(start, Vector3.SqrMagnitude(start - dest));

        Vector3 current;
        debugger.Clear();
        while(openSet.Count != 0) {
            current = openSet.MinBy(x => GetScore(fScore, x)).First();
            if(Vector3.SqrMagnitude(current - dest) <= 2 * 2f) {
                return ReconstructPath(current);
            }
            openSet.Remove(current);
            closedSet.Add(current);


            List<Vector3> neighbors = GenerateNeighbors(current);
            debugger.DrawPoints(neighbors, Color.green);
            foreach (Vector3 neighbor in neighbors) {
                if(closedSet.Contains(neighbor)) {
                    continue;
                }
                var neighDist = Vector3.SqrMagnitude(current - neighbor);

                var filter = new ContactFilter2D();
                filter.NoFilter();

                RaycastHit2D[] results = new RaycastHit2D[100];
                int hitCount = Physics2D.Raycast(new Vector2(current.x, current.y), new Vector2(neighbor.x, neighbor.y), filter, results, neighDist);
                if(hitCount > 0) {
                    bool skipPoint = false;
                    for(int i = 0; i < hitCount; i++) {
                        if (results[i].collider.tag == "Mineable") {
                            Debug.Log($"Got raycast hit at {results[i].collider.name}");
                            closedSet.Add(neighbor);
                            skipPoint = true;
                            break;
                        }
                    }
                    if (skipPoint)
                        continue;
                }
                if(!openSet.Contains(neighbor)) {
                    openSet.Add(neighbor);
                }
                float tentativeGScore = GetScore(gScore, current);
                tentativeGScore += neighDist; 
                if (tentativeGScore >= GetScore(gScore, neighbor)) {
                    continue;
                }
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = (float)gScore[neighbor] + Vector3.SqrMagnitude(neighbor - dest);
            }
        }
        return null;
    }

    private List<Vector3> ReconstructPath(Vector3 current) {
        List<Vector3> path = new List<Vector3>();
        path.Add(current);
        while(cameFrom.ContainsKey(current)) {
            current = (Vector3)cameFrom[current];
            path.Add(current);
        }
        for(int i = 1; i < path.Count; i++) {
            debugger.DrawLine(path[i - 1], path[i], Color.cyan);
        }
        return path;
    }

    private void Awake() {
        closedSet = new HashSet<Vector3>();
        openSet = new List<Vector3>(); 
        cameFrom = new Dictionary<Vector3, Vector3>();
        gScore = new Dictionary<Vector3, float>();
        fScore = new Dictionary<Vector3, float>();
        debugger = GetComponent<Debugger>();
        if(debugger == null) {
            debugger = gameObject.AddComponent<Debugger>();
        }
    }
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
