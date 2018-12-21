using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myBurnObject : MonoBehaviour {

    public float heat = 2f;
    private void OnTriggerStay(Collider other)
    {
		var objectStatus = other.transform.gameObject.GetComponent<myObjectStatus>();
    	if (objectStatus) objectStatus.HeatUp(heat);
    }
}
