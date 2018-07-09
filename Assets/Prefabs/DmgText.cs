using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgText : MonoBehaviour {
    [Range(0, 5)]
    public float aliveTimer = 3f;
    public float speed = 3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        aliveTimer -= Time.deltaTime;

        if(aliveTimer <= 0)
        {
            Destroy(gameObject);
        } else
        {
            this.transform.position += new Vector3(0, 1, 0) * Time.deltaTime * speed;
        }
	}
}
