using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myExtinguishObject : MonoBehaviour {


    private void OnTriggerStay(Collider other)
    {
        
        var objectStatus = other.transform.gameObject.GetComponent<myObjectStatus>();
        if (objectStatus) objectStatus.SetExtinguish();
    }
}