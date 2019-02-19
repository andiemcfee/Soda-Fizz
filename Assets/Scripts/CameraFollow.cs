using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    [SerializeField] Vector3 offset;

	// Use this for initialization
	void Start() 
    {
		
	}

	void LateUpdate()
	{
        transform.position = target.transform.position + offset;
	}
}
