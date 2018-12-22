using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPeople : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.transform.gameObject;
        if (otherPlayer.tag == "Lighter") {
        	var ctrl = other.transform.gameObject.GetComponent<myPlayerController>();
        	ctrl.CmdKilled(otherPlayer);
        }
    }
}