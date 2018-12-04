using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class myFire : NetworkBehaviour {

    private Transform ps;

	// Use this for initialization
	void Start () {
        ps = transform.Find("LighterFlame");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ps.gameObject.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            ps.gameObject.SetActive(false);
        }
	}
}
