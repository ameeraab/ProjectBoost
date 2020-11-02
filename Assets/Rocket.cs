using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip deathSound;

    Rigidbody rigidBody;
    AudioSource audioSource;
    static int scene = 0;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        //audioSource.Stop();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        else if(state == State.Dying)
        {
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
            return;

        float delay = 3.0f;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartWinSequence(delay);
                break;
            default:
                StartDeathSequence(delay);
                break;
        }
    }

    private void StartWinSequence(float delay)
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        Invoke(nameof(LoadNextScene), delay);
    }

    private void StartDeathSequence(float delay)
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke(nameof(LoadSameScene), delay);
    }

    private void LoadNextScene()
    {
        state = State.Alive;

        audioSource.Stop();

        if (scene < SceneManager.sceneCountInBuildSettings - 1)
            scene++;
        SceneManager.LoadScene(scene);
    }

    private void LoadSameScene()
    {
        state = State.Alive;

        audioSource.Stop();

        SceneManager.LoadScene(scene);
    }

    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}
