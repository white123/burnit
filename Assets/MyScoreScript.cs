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
    void Start()
    {
        totalHealth = 0f;
        all = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject a in all)
        {
            totalHealth += a.GetComponent<myObjectStatus>().maxHealth;
        }
    }
    void Update()
    {
        curTotalHealth = 0;
        foreach (GameObject a in all)
        {
            curTotalHealth += a.GetComponent<myObjectStatus>().getCurHealth();
        }
        scoreText.text =  (curTotalHealth / totalHealth).ToString("P");
    }
}
