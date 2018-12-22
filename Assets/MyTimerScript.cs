using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MyTimerScript : NetworkBehaviour {

    public Text uiText;
    public float gameTime;
    [SyncVar]
    private float timer;
    private Color red;
    private Color yello;
	// Use this for initialization
	void Start () {
        timer = gameTime;
        red = new Color(1f, 0f, 0f);
        yello = new Color(1f, 250f / 255f, 205f / 255f);

    }
	
	// Update is called once per frame
	void Update () {
		if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer >= 60f)
            {
                string minute = ((int)timer / 60).ToString("D2");
                string second = ((int)timer % 60).ToString("D2");
                uiText.text = minute + ":" + second;
            }
            else
            {
                uiText.text = timer.ToString("F");
                if (timer < 10f) uiText.color = red;
                else if (timer < 60f) uiText.color = yello;
            }
            
        }
        else
        {
            timer = 0;
            uiText.text = timer.ToString("F");
        }
	}
}
