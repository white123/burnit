using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class myGameManeger : NetworkBehaviour {

    
    public Text healthText;
    public Text timeText;
    public Text leftText;
    public Text startText;
    public GameObject gameoverCanvas;
    public float gameTime;
    public float waitTime;

    [SyncVar]
    private float timer;
    private float totalHealth;
    [SyncVar]
    private float curTotalHealth;
    private GameObject[] objects;
    private GameObject[] players;
    private int lighterCount;
    private bool start;
    private bool over;
    private float startTime;
    private Color red;
    private Color yello;
    
    

	// Use this for initialization
	void Start () {
        //set timeText to maxTime
        timer = gameTime;
        string minute = ((int)timer / 60).ToString("D2");
        string second = ((int)timer % 60).ToString("D2");
        timeText.text = minute + ":" + second;

        //find all Object and calculate maxHealth
        totalHealth = 0f;
        objects = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject o in objects)
        {
            totalHealth += o.GetComponent<myObjectStatus>().maxHealth;
        }
        curTotalHealth = totalHealth;

        //initiate color
        red = new Color(1f, 0f, 0f);
        yello = new Color(1f, 250f / 255f, 205f / 255f);

        //set game not yet start
        start = false;
        over = false;
        startTime = waitTime;
    }
	
	// Update is called once per frame
	void Update () {
        //if game over
        if (over) return;

        //if game not yet start
        if (!start)
        {
            if (startTime > 0)
            {
                startText.text = ((int)startTime).ToString("D");
                startTime -= Time.deltaTime;
            }
            else
            {
                startText.text = "00";
                Destroy(startText);
                start = true;
                int count = findAllPlayer();
                //random one to extinguisher
                if (isServer)
                {
                    int extinguisher = Random.Range(0, count);
                    players[extinguisher].GetComponent<FirstPersonController>().SetSpeed(12f, 24f);
                    players[extinguisher].GetComponent<myPlayerController>().CmdChange();
                }
                
            }
            return;
        }
        //update timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer >= 60f)
            {
                string minute = ((int)timer / 60).ToString("D2");
                string second = ((int)timer % 60).ToString("D2");
                timeText.text = minute + ":" + second;
            }
            else
            {
                timeText.text = timer.ToString("F");
                if (timer < 10f) timeText.color = red;
                else if (timer < 60f) timeText.color = yello;
            }

        }
        else
        {
            timeText.text = "0";
            gameover();
            return;
        }

        //update lighter left
        lighterCount = 0;
        foreach(GameObject player in players)
        {
            if(player.tag == "Lighter")
            {
                lighterCount++;
            }
        }
        leftText.text = lighterCount.ToString("D2");
        if(lighterCount == 0)
        {
            gameover();
            return;
        }
        
        //update totalHealth
        curTotalHealth = 0;
        foreach (GameObject o in objects)
        {
            curTotalHealth += o.GetComponent<myObjectStatus>().getCurHealth();
        }
        float health = (curTotalHealth / totalHealth);
        healthText.text = health.ToString("P");

        if (health < 0.8f) healthText.color = yello;
        if (health < 0.3f) healthText.color = red;

        if(health <= 0)
        {
            healthText.text = "0%";
            gameover();
            return;
        }

    }

    private int findAllPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Lighter");
        foreach(GameObject player in players)
        {
            player.transform.Find("MyLighter").GetComponent<myBurnObject>().setEnable();
        }
        return players.Length;
    }

    private void gameover()
    {
        foreach(GameObject canvas in GameObject.FindGameObjectsWithTag("canvas"))
        {
            Destroy(canvas);
        }
        GameObject go = Instantiate(gameoverCanvas, Vector2.zero, Quaternion.identity);
        Text health = go.transform.Find("health").GetComponent<Text>();
        float h = ((int)(1000*(curTotalHealth / totalHealth))) / 10;
        health.text = "HEALTH: " + h.ToString() + "%";

        Text text = go.transform.Find("text").GetComponent<Text>();

        bool isLighter = false;
        foreach(GameObject player in players)
        {
            if(player.name == "ME")
            {
                if (player.tag == "Lighter" || player.tag == "Dead") isLighter = true;
                break;
            }
        }
        
        if (h > 0.1f)
        {
            if (isLighter) text.text = "YOU LOSE!";
            else text.text = "YOU WIN!";
        }
        else
        {
            if (isLighter) text.text = "YOU WIN!";
            else text.text = "YOU LOSE!";
        }
        
        

        over = true;
        Time.timeScale = 0;

        AudioSource source = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        //source.Stop();
        source.pitch = 0.6f;
        //source.Play();
    }
}
