using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myBurnObject : MonoBehaviour {

    
    private void OnTriggerStay(Collider other)
    {

        var objectStatus = other.transform.gameObject.GetComponent<myObjectStatus>();
        if (objectStatus) objectStatus.SetBurning();
    }
}
