using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour, IKillable {
    public GameObject playerShip;
    public Text dmgText;
    float hp = 10;

    // Use this for initialization
    void Start () {
        playerShip = GameObject.Find("mainPlayer");
    }

    // Update is called once per frame
    void Update () {
    }

    void OnMouseDown() {
        var mp = playerShip.GetComponent<MainPlayer>();
        mp.SetTarget(gameObject);
        ApplyDamage(1);
    }

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

        if(hp <= 0) Kill();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
