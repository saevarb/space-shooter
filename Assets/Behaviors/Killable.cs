using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Killable : MonoBehaviour {

    public float hp;
    public Text dmgText;
    // Use this for iprivate nitialization
    public void ApplyDamage(float dmg)
    {
        GameObject canvasObject = GameObject.Find("dmgCanvas");
        Canvas dmgCanvas = canvasObject.GetComponent<Canvas>();

        GameObject tObj = Instantiate(dmgText.gameObject) as GameObject;
        Text t = tObj.GetComponent<Text>();
        t.text = $"-{dmg}";
        Debug.Log(transform.position);
        tObj.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
        Debug.Log(tObj.transform.position);
        tObj.transform.SetParent(dmgCanvas.transform);

        hp -= dmg;
        if(hp <= 0) Kill();
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
