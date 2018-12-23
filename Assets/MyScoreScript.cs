using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyScoreScript : NetworkBehaviour {

    public Text scoreText;

    private float totalHealth;
    [SyncVar]
    private float curTotalHealth;
    private GameObject[] all;
    private Color red;
    private Color yello;
    void Start()
    {
        totalHealth = 0f;
        all = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject a in all)
        {
            totalHealth += a.GetComponent<myObjectStatus>().maxHealth;
        }
        red = new Color(1f, 0f, 0f);
        yello = new Color(1f, 250f / 255f, 205f / 255f);
    }
    void Update()
    {
        curTotalHealth = 0;
        foreach (GameObject a in all)
        {
            curTotalHealth += a.GetComponent<myObjectStatus>().getCurHealth();
        }
        float health = (curTotalHealth / totalHealth);
        scoreText.text =  health.ToString("P");

        if (health < 0.6f) scoreText.color = yello;
        if (health < 0.3f) scoreText.color = red;
    }
}
