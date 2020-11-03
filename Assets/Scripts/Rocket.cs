using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float levelLoadDelay = 0;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem SuccessParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    static int scene = 0;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    bool isCollisionEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Debug.isDebugBuild)
            RespondToDebugKeys();

        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isCollisionEnabled = !isCollisionEnabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !isCollisionEnabled)
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartWinSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartWinSequence()
    {
        state = State.Transcending;

        audioSource.Stop();
        audioSource.PlayOneShot(winSound);

        mainEngineParticles.Stop();
        SuccessParticles.Play();

        Invoke(nameof(LoadNextScene), levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;

        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);

        mainEngineParticles.Stop();
        deathParticles.Play();

        Invoke(nameof(LoadSameScene), levelLoadDelay);
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
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);

        mainEngineParticles.Play();
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
