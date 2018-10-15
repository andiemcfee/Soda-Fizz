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
    [SerializeField] float timeBetweenLvls = 1f;
    enum State { Dying, Alive, Transcending }
    State state = State.Alive;

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
                state = State.Transcending;
                Invoke("loadNextScene", timeBetweenLvls);
                break;
			default:
					//do nothing
					break;
		}
	}

	void loadNextScene() //loads next scene using a timer
    {
        SceneManager.LoadScene(currentLevel + 1);
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

        if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow))
        {
            rigidBody.AddTorque(transform.right * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow))
        {
            rigidBody.AddTorque(-transform.right * rotationThisFrame);
        }
    }
}