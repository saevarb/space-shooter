using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpDisplay : MonoBehaviour {

    private Text hpText;
    private HasHealth killable;
	// Use this for initialization
	void Awake () {
        GameObject canvasObject = GameObject.Find("dmgCanvas");
        Canvas dmgCanvas = canvasObject.GetComponent<Canvas>();

        killable = GetComponent<HasHealth>();

        if(killable != null) {
            GameObject tObj = new GameObject();

            tObj.transform.SetParent(dmgCanvas.transform);

            hpText = tObj.AddComponent<Text>();

            UpdateText();

            hpText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            hpText.rectTransform.sizeDelta = new Vector2(10, 10);
            hpText.horizontalOverflow = HorizontalWrapMode.Overflow;
            hpText.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }

    private void UpdateText() {
        if(killable != null) {
            hpText.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-.15f, .08f, 0));
            hpText.text = killable.health.ToString("0.0");
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateText();
	}
    private void OnDestroy() {
        if(killable != null) {
            if(hpText != null)
                Destroy(hpText.gameObject);
        }
    }
}
