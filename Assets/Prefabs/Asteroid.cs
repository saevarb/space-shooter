using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour {
    private GameObject playerShip;
    private Text hpText;
    private Killable killable;

    // Use this for initialization
    void Start () {
        playerShip = GameObject.Find("mainPlayer");
        GameObject canvasObject = GameObject.Find("dmgCanvas");
        Canvas dmgCanvas = canvasObject.GetComponent<Canvas>();

        GameObject tObj = new GameObject();
        tObj.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-.15f, .08f, 0));
        tObj.transform.SetParent(dmgCanvas.transform);
        killable = GetComponent<Killable>();

        hpText = tObj.AddComponent<Text>();
        hpText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        hpText.rectTransform.sizeDelta = new Vector2(10, 10);
        hpText.horizontalOverflow = HorizontalWrapMode.Overflow;
        hpText.verticalOverflow = VerticalWrapMode.Overflow;

        hpText.text = killable.hp.ToString("0.0");
    }

    // Update is called once per frame
    void Update () {
        hpText.text = killable.hp.ToString("0.0");
    }

    void OnMouseDown() {
        var mp = playerShip.GetComponent<MainPlayer>();
        mp.SetTarget(gameObject);
    }

    private void OnDestroy() {
        Destroy(hpText); 
    }

}
