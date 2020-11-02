using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1000f;

    Rigidbody rigidBody;
    AudioSource thrustNoise;
    static int scene = 0;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        thrustNoise = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        else if(state == State.Dying)
        {
            thrustNoise.Stop();
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
                print("Hit friendly");
                break;
            case "Finish":
                print("Hit finish");
                state = State.Transcending;
                Invoke(nameof(LoadNextScene), delay);
                break;
            default:
                print("Dead");
                state = State.Dying;
                Invoke(nameof(LoadSameScene), delay);
                LoadSameScene();
                break;
        }
    }

    private void LoadSameScene()
    {
        state = State.Alive;
        SceneManager.LoadScene(scene);
    }

    private void LoadNextScene()
    {
        state = State.Alive;

        if (scene < SceneManager.sceneCountInBuildSettings - 1)
            scene++;
        
        SceneManager.LoadScene(scene);
    }

    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!thrustNoise.isPlaying)
                thrustNoise.Play();
        }
        else
        {
            thrustNoise.Stop();
        }
    }

    private void Rotate()
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
