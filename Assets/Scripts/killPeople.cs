using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPeople : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.transform.gameObject;
        if (otherPlayer.tag == "Lighter" && 
        	this.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<myPlayerController>().iisLocalPlayer()) {

        	var ctrl = other.transform.gameObject.GetComponent<myPlayerController>();
        	ctrl.CmdKilled();
        }
    }
}