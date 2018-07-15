using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HasHealth : MonoBehaviour {

    public float health;
    public Text dmgText;
    private Text lastText = null;
    private float? lastDamageTime;
    private float? lastDamage = null;

    private void CreateDamageNumber(float dmg) {
        GameObject canvasObject = GameObject.Find("dmgCanvas");
        Canvas dmgCanvas = canvasObject.GetComponent<Canvas>();
        GameObject tObj = Instantiate(dmgText.gameObject) as GameObject;
        Text t = tObj.GetComponent<Text>();
        t.text = $"-{dmg.ToString("0.0")}";
        tObj.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
        tObj.transform.SetParent(dmgCanvas.transform);
        lastText = t;
        lastDamageTime = Time.time;
        lastDamage = dmg;
    }

    public void ApplyDamage(float dmg)
    {
        if(lastDamageTime != null && lastText != null) {
            float timeSinceLastDamage = Time.time - (float)lastDamageTime;
            if(timeSinceLastDamage <= 3) {
                lastDamage += dmg;
                lastText.text = "-" + lastDamage?.ToString("0.0");
                lastDamageTime = Time.time;
            } else {
                Debug.Log($"Creating damage numbers because delay was {timeSinceLastDamage}");
                CreateDamageNumber(dmg);
            }
        } else {
            Debug.Log("Creating damage numbers because no last time");
            CreateDamageNumber(dmg);
        }
        health -= dmg;
        if(health <= 0) Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
