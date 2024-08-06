using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float mainThrust = 1000f;
    [SerializeField] private float rotationThrust = 100f;
    [SerializeField] AudioClip thrusters;

    [SerializeField] ParticleSystem leftThruster;
    [SerializeField] ParticleSystem rightThruster;
    [SerializeField] ParticleSystem mainThruster;

    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            MainThrust();
        }
        else
        {
            StopMainThrust();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotation();
        }
    }

    private void MainThrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrusters);
        }
        if (!mainThruster.isPlaying)
        {
            mainThruster.Play();
        }
    }


    private void StopMainThrust()
    {
        mainThruster.Stop();
        audioSource.Stop();
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThruster.isPlaying)
        {
            leftThruster.Play();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightThruster.isPlaying)
        {
            rightThruster.Play();
        }
    }

    private void StopRotation()
    {
        rightThruster.Stop();
        leftThruster.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //Freeze rotation
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
