using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    public Rigidbody rigidBody;
    public AudioSource audioSource;

    private int currentLevel = 0;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rcsRotationThrust = 100f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowFallMultiplier = 2f;

	void Start ()
    {
        audioSource = GetComponent< AudioSource > ();
        rigidBody = GetComponent<Rigidbody>();
	}

    void Update() //calls hardFall, thrust, and rotate each frame
    {
        hardFall();
        Thrust();
        Rotate();

    }

    void hardFall() //applies extra gravity when the soda bottle is falling
    {
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowFallMultiplier - 1) * Time.deltaTime;
        }
    }

	void OnCollisionEnter(Collision collision) //switches levels
	{
		switch (collision.gameObject.tag)
		{
				case "Finish":
                SceneManager.LoadScene(currentLevel + 1);
					break;
				default:
					//do nothing
					break;
		}
	}

	void Thrust()//handles upward movement and audio
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

    void Rotate() //handles user rotation
    {
        float rotationThisFrame = rcsRotationThrust * Time.deltaTime;

        //rigidBody.freezeRotation = true; //allows user to take sole manual control of rotation

        if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Rotate(Vector3.forward * rotationThisFrame);
            rigidBody.AddTorque(transform.right * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow))
        {
            //transform.Rotate(-Vector3.forward* rotationThisFrame);
            rigidBody.AddTorque(-transform.right * rotationThisFrame);
        }
        //rigidBody.freezeRotation = false; 
    }
}