using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myObjectStatus : MonoBehaviour {

    public Transform fire;
    public Material burnedMat;
    public float maxHealth = 100;

    private bool isBurning = false;
    private float curHealth;

	// Use this for initialization
	void Start () {
        curHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        if (isBurning)
        {
            if(curHealth > 0)
            {
                curHealth -= 1 * Time.deltaTime;
                Debug.Log(curHealth);
            }
            else
            {
                SetExtinguish();
                ChangeMaterial(burnedMat);
                Debug.Log("dead");
            }

        }
	}

    public void SetBurning()
    {
        if (!isBurning && curHealth > 0)
        {
            isBurning = true;
            this.transform.Find("MyFire").gameObject.SetActive(true);
        }
    }
    public void SetExtinguish()
    {
        if (isBurning)
        {
            isBurning = false;
            this.transform.Find("MyFire").gameObject.SetActive(false);
        }
    }

    void ChangeMaterial(Material newMat)
    {
        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in children)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = newMat;
            }
            rend.materials = mats;
        }
    }
}
