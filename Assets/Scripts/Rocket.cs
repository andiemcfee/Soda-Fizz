using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    public Rigidbody rigidBody;
    public AudioSource audioSource;
    [SerializeField] ParticleSystem fizz;

    int currentLevel;
    int currentBuildIndex;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rcsRotationThrust = 100f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowFallMultiplier = 2f;
    [SerializeField] float timeBetweenLvls = 1f;
    [SerializeField] int sodaLeft = 500;

    enum State { Dying, Alive, Transcending }
    State state = State.Alive;

	void Start ()
    {
        audioSource = GetComponent< AudioSource > ();
        rigidBody = GetComponent<Rigidbody>();
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);
        currentBuildIndex = scene.buildIndex;
	}

    void Update() //calls hardFall, thrust, and rotate each frame
    {
        hardFall();
        Thrust();
        Rotate();
        depleteSoda();

    }

    void depleteSoda()
    {
        if (Input.GetKey(KeyCode.Space) && sodaLeft > 0)
        {
            sodaLeft -= 2;
        }
        else if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)
                 | Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow) && sodaLeft > 0)
        {
            sodaLeft -= 1;
        }
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
                Invoke("loadNextLevel", timeBetweenLvls);
                break;
            case "Unfriendly":
                state = State.Transcending;
                Invoke("restartLevel", timeBetweenLvls);
                break;
			default:
					//do nothing
					break;
		}
	}

	void loadNextLevel() //loads next scene using a timer
    {
        currentLevel = currentBuildIndex + 1;
        if (currentLevel == 3)
        {
            currentLevel = 0;
        }
        SceneManager.LoadScene(currentLevel, LoadSceneMode.Single);
        state = State.Transcending;
    }

    void restartLevel() //reloads the same scene if the player "dies"
    {
        SceneManager.LoadScene(currentBuildIndex, LoadSceneMode.Single);
    }

    void Thrust()//handles upward movement and audio
    {
        float thrustThisFrame = rcsThrust * Time.deltaTime;

        if (sodaLeft > 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
                fizz.Play();

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
                fizz.Stop();
            }
        }
        else if (sodaLeft <= 100)
        {
            rigidBody.mass = 0;
        }
    }

    void Rotate() //handles user rotation
    {
        float rotationThisFrame = rcsRotationThrust * Time.deltaTime;

        if (sodaLeft > 0)
        {
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
}