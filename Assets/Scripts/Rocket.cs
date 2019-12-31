using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Rocket : MonoBehaviour {

    public Rigidbody rigidBody;
    public AudioSource audioSource;
    [SerializeField] ParticleSystem fizz;
   
    public CameraShake shake;


    int currentLevel;
    int currentBuildIndex;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rcsRotationThrust = 100f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowFallMultiplier = 2f;
    [SerializeField] float timeBetweenLvls = 1f;
    [SerializeField] int sodaLeft = 5000;
    [SerializeField] float thrustMultiplier = 1.75f;
    [SerializeField] float rotationMultiplier = 1.5f;



    //public int scoreNum;

    enum State { Dying, Alive, Transcending }
    State state = State.Alive;
    private bool firstFountainEnter = true;
    private bool isPlayable = false;

	void Start ()
    {
        //grabbing components
        audioSource = GetComponent<AudioSource> ();
        rigidBody = GetComponent<Rigidbody>();
        GameObject.Find("Static Main Camera").GetComponent<CameraShake>();
        //scoreNum = GameObject.Find("Static Main Camera").GetComponent<ScoringSys>().scoreNum;

        //scene swapping stuff
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);
        currentBuildIndex = scene.buildIndex;

        isPlayable = false;
        Invoke("MakePlayable", 3f);
	}

    void Update() //calls hardFall, thrust, and rotate each frame
    {
        hardFall();
        if (isPlayable == true)
        {
            Thrust();
            Rotate();
        }
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

	void OnCollisionEnter(Collision collision) //switches levels, applies extra force going through fountains
	{
		switch (collision.gameObject.tag)
		{
			case "Finish":
                Invoke("loadNextLevel", timeBetweenLvls);
                shake.Shake(shake.shakeDuration);
                shake.isShaking = false;
                isPlayable = false;
                //scoreNum = 5000 - sodaLeft;
                break;
            case "Unfriendly":
                state = State.Transcending;
                Invoke("restartLevel", timeBetweenLvls);
                break;
            case "sodafountain":
                if (firstFountainEnter == true)
                {
                    rcsThrust = (rcsThrust * thrustMultiplier);
                    rcsRotationThrust = (rcsRotationThrust * rotationMultiplier);
                    rigidBody.mass = 0.2f;
                    Debug.Log("In Fountain!");
                    firstFountainEnter = false;
                }
                break;
            default:
                //do nothing
                break;
        }
	}

	void loadNextLevel() //loads next scene using a timer
    {
        currentLevel = currentBuildIndex + 1;
        if (currentLevel == 6)
        {
            currentLevel = 0;
        }
        isPlayable = false;
        SceneManager.LoadScene(currentLevel, LoadSceneMode.Single);
        Invoke("MakePlayable", 3f);

        state = State.Transcending;
    }

    void restartLevel() //reloads the same scene if the player "dies"
    {
        SceneManager.LoadScene(currentBuildIndex, LoadSceneMode.Single);
    }

    void MakePlayable()
    {
        isPlayable = true;
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

                //if (!audioSource.isPlaying)
                {
                    //audioSource.Play();
                }
            }
            else
            {
                //audioSource.Stop();
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

        if (sodaLeft > 0)
        {
            if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow))
            {
                rigidBody.AddTorque(transform.right * (rcsRotationThrust * 1000));
            }
            else if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow))
            {
                rigidBody.AddTorque(-transform.right * (rcsRotationThrust * 1000));
            }
        }
    }
    
}