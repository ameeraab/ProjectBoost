using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Move platforms
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;

    [Range(0,1)][SerializeField] float movementFactor; //0 for not moved, 1 for fully moved 1.9*10 = 19 Y is the top

    private Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
