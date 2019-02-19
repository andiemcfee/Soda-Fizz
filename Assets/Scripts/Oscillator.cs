using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor;
    Vector3 startingPos;

	// Use this for initialization
	void Start () 
    {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (period >= 0.5)
        {
            float cycles = Time.time / period;

            const float tau = Mathf.PI * 2f;
            float rawSinWave = Mathf.Sin(cycles * tau);

            movementFactor = rawSinWave / 2f + 0.5f;
            Vector3 offset = movementFactor * movementVector;
            transform.position = startingPos + offset;
        }
	}
}
