using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myBurnObject : MonoBehaviour {

    public float heat = 2f;

    private bool canBurn = false;

    private void OnTriggerStay(Collider other)
    {
        if (canBurn)
        {
            var objectStatus = other.transform.gameObject.GetComponent<myObjectStatus>();
            if (objectStatus) objectStatus.HeatUp(heat);
        }
		
    }

    public void setEnable()
    {
        canBurn = true;
    }
}
