using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public Rigidbody rigidBody;
    public AudioSource audioSource;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rcsRotationThrust = 100f; 

	void Start () 
    {
        audioSource = GetComponent< AudioSource > ();
        rigidBody = GetComponent<Rigidbody>(); //important!!
	}

    void Update()
    {
        Thrust();
        Rotate();

    }

	private void OnCollisionEnter(Collision collision)
	{
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Youre OK!"); //todo remove line
                //do nothing
                break;
            default:
                Debug.Log("Dead!"); //todo remove line
                //kill player, quickly restart level
                break;
        }
	}

	void Thrust()
    {
        float thrustThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    void Rotate()
    {
        float rotationThisFrame = rcsRotationThrust * Time.deltaTime;

        rigidBody.freezeRotation = true; //allows user to take manual control of rotation

        if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward* rotationThisFrame);
        }
        rigidBody.freezeRotation = false; 
    }
}