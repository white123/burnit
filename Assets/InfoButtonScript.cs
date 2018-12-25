using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OpenInfo()
    {
        transform.Find("DirectPlaySubPanel/InfoPage").gameObject.SetActive(true);
    }
    public void CloseInfo()
    {
        GameObject.FindGameObjectWithTag("Info").SetActive(false);
    }
}
