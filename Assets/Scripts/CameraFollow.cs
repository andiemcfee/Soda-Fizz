using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    [SerializeField] float offset;

	void LateUpdate()
	{
        transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
        //transform.position.x = target.transform.position.x + offset;
    }
}
