using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Move platforms
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    private float movementFactor; //0 for not moved, 1 for fully moved 1.9*10 = 19 Y is the top

    private Vector3 startingPos;
    [SerializeField] float period = 2f;

    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period == 0)
            return;

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
