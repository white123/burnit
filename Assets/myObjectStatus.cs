using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myObjectStatus : MonoBehaviour {

    public Transform fire;
    public Material burnedMat0;
    public Material burnedMat25;
    public Material burnedMat50;
    public Material burnedMat75;

    public float maxHealth = 10000;
    public float specificHeat = 1;

    private bool isBurning = false;
    private bool isSmoking = false;
    private bool isDead = false;
    private const float MAX_T = 100;
    private const float MIN_T = 0;
    private const float FIRE_POINT = 30;
    private float curHealth;
    private float temperature = 0;

	// Use this for initialization
	void Start () {
        curHealth = maxHealth;
	}

    // Update is called once per frame
    void Update() {

        if(curHealth > 0)
        {
            if (temperature > FIRE_POINT && temperature <= MAX_T)
            {
                HeatUp(0.1f);
                if (!isBurning) SetBurn();
                
            }
            else if (temperature <= FIRE_POINT && temperature > MIN_T)
            {
                CoolDown(0.1f);
                if (isBurning) SetExtinguish();
                if (!isSmoking) SetSmoke();
            }
            else if (temperature <= MIN_T)
            {
                if (isSmoking) SetSmokeOff();
            }

            if (temperature > MIN_T) curHealth -= temperature * Time.deltaTime;
            if(curHealth <= 0.25*maxHealth)
            {
                ChangeMaterial(burnedMat25);
            }
            else if(curHealth <= 0.50*maxHealth)
            {
                ChangeMaterial(burnedMat50);
            }
            else if(curHealth <= 0.75*maxHealth)
            {
                ChangeMaterial(burnedMat75);
            }

        }
        else
        {
            if (!isDead)
            {
                SetExtinguish();
                SetSmokeOff();
                ChangeMaterial(burnedMat0);
                isDead = true;
            }
            
        }
	}

    public void HeatUp(float heat)
    {
        if(temperature < MAX_T)
        {
            temperature += heat / specificHeat;
        }
    }
    public void CoolDown(float heat)
    {
        if (temperature > MIN_T)
        {
            temperature -= heat / specificHeat;
        }
    }

    private void SetBurn()
    {
        isBurning = true;
        this.transform.Find("MyFire").gameObject.SetActive(true);
        
    }
    private void SetExtinguish()
    {
        isBurning = false;
        this.transform.Find("MyFire").gameObject.SetActive(false);
        
    }
    private void SetSmoke()
    {
        isSmoking = true;
        this.transform.Find("Smoke").gameObject.SetActive(true);
    }
    private void SetSmokeOff()
    {
        isSmoking = false;
        this.transform.Find("Smoke").gameObject.SetActive(false);
    }

    void ChangeMaterial(Material newMat)
    {
        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            if (rend.name == "Smoke") continue;
            if (rend.name == "MyFire") continue;
            if (rend.transform.parent.name == "MyFire") continue;
            
            
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = newMat;
            }
            rend.materials = mats;
        }
    }
}
