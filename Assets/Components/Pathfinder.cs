using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoreLinq;

[RequireComponent(typeof(Collider2D))]
public class Pathfinder : MonoBehaviour {
    [Range(1, 60)]
    public int neighborCount = 5;
    [Range(.1f, 3)]
    public float stepSize = 1;
    public bool debug = false;

    private HashSet<Vector3> closedSet;
    private SortedList<float, Vector3> openSet;
    private Dictionary<Vector3, Vector3> cameFrom;
    private Dictionary<Vector3, float> gScore;
    private Dictionary<Vector3, float> fScore;
    private Debugger debugger;

    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable {
        public int Compare(TKey x, TKey y) {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;   // Handle equality as beeing greater
            else
                return result;
        }
    }

    private Vector3 ClosestOnGrid(Vector3 p) {
        return GridAround(p).MinBy(x => (p - x).sqrMagnitude).First();
    }

    private IEnumerable<Vector3> GridAround(Vector3 p) {
        var start = new Vector3((int)(p.x), (int)(p.y), p.z);
        for (float i = 0; i < 1; i += stepSize) {
            for (float j = 0; j < 1; j += stepSize) {
                yield return start + new Vector3(i, j, 0);
            }
        }
    }
    // Use this for initialization
    private List<Vector3> GenerateNeighbors(Vector3 cur) {
        List<Vector3> neighbors = new List<Vector3>();
        var start = new Vector3(Mathf.Round(cur.x), Mathf.Round(cur.y), cur.z);
        float offset = neighborCount * stepSize;
        for (float i = -offset; i < offset; i += stepSize) {
            for (float j = -offset; j < offset; j += stepSize) {
                neighbors.Add(start + new Vector3(i, j, 0));
            }
        }
        //for (float angle = 0; angle <= Mathf.PI * 2; angle += 2 * Mathf.PI / neighborCount) {
        //    Vector3 v = stepSize * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        //    //Debug.DrawRay(cur, v, Color.green);
        //    neighbors.Add(cur + v);
        //}
        return neighbors;
    }

    private Queue<Vector3> ReconstructPath(Vector3 current) {
        Stack<Vector3> pathStack = new Stack<Vector3>(); ;
        do {
            pathStack.Push(current);
            if(!cameFrom.ContainsKey(current)) {
                Debug.LogWarning($"Could not find {current} in cameFrom");
                break;
            }
            current = (Vector3)cameFrom[current];
        } while (cameFrom.ContainsKey(current));
        return new Queue<Vector3>(pathStack);
    }

    private float GetScore(Dictionary<Vector3, float> scoreTable, Vector3 key) {
        if (scoreTable.ContainsKey(key)) {
            return scoreTable[key];
        } else {
            return float.MaxValue / 2;
        }
    }

    public Queue<Vector3> FindPath(Vector3 dest) {
        closedSet.Clear();
        openSet.Clear();
        cameFrom.Clear();
        gScore.Clear();
        fScore.Clear();
        var start = transform.position;
        start = ClosestOnGrid(start);

        gScore.Add(start, 0);
        fScore.Add(start, Vector3.SqrMagnitude(start - dest));
        openSet.Add(GetScore(fScore, start), start);

        Vector3 current;
        debugger.Clear();
        int explored = 0;
        while (openSet.Count != 0) {
            current = openSet.First().Value;
            openSet.RemoveAt(0);

            if (openSet.ContainsValue(current))
                continue;
            explored++;
            if (debug) {
                //debugger.DrawPoints(neighbors, Color.white);
                debugger.DrawPoint(current, Color.black);
            }

            if (Vector3.SqrMagnitude(current - dest) <= 1) {
                Debug.Log("Nodes explored: " + explored);
                return ReconstructPath(current);
            }

            closedSet.Add(current);

            List<Vector3> neighbors = GenerateNeighbors(current);
            foreach (Vector3 neighbor in neighbors) {
                if (closedSet.Contains(neighbor)) {
                    continue;
                }
                var neighDist = Vector3.SqrMagnitude(current - neighbor);
                var neighHeading = neighbor - current;

                var filter = new ContactFilter2D();
                filter.NoFilter();

                //int hitCount = Physics2D.Raycast(new Vector2(current.x, current.y), new Vector2(neighbor.x, neighbor.y), filter, results, neighDist);
                var foo = GetComponent<CircleCollider2D>();
                RaycastHit2D hit = Physics2D.CircleCast(new Vector2(current.x, current.y), foo.radius, neighHeading, neighDist);
                if (hit && hit.collider.gameObject != this.gameObject) {
                    closedSet.Add(neighbor);
                    continue;
                }
                float tentativeGScore = GetScore(gScore, current);
                tentativeGScore += neighDist;
                if (tentativeGScore >= GetScore(gScore, neighbor)) {
                    continue;
                }
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = (float)gScore[neighbor] + Vector3.SqrMagnitude(neighbor - dest);
                if (!openSet.ContainsValue(neighbor)) {
                    openSet.Add(GetScore(fScore, neighbor), neighbor);
                }
            }
        }
        return null;
    }

    private void Awake() {
        closedSet = new HashSet<Vector3>();
        openSet = new SortedList<float, Vector3>(new DuplicateKeyComparer<float>());
        cameFrom = new Dictionary<Vector3, Vector3>();
        gScore = new Dictionary<Vector3, float>();
        fScore = new Dictionary<Vector3, float>();
        debugger = GetComponent<Debugger>();
        if (debugger == null) {
            debugger = gameObject.AddComponent<Debugger>();
        }
    }
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }
}
