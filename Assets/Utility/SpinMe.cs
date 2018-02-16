using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour {

    [SerializeField] Vector3 RotationsPerMinute = new Vector3(1f, 1f, 1f);
	
	void Update ()
    {
        Vector3 degrees = ((360 * Time.deltaTime) / 60) * RotationsPerMinute;
        transform.Rotate(degrees);
    }
}
