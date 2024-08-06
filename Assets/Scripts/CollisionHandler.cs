using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] AudioClip collisionSound;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem collisionParticle;
    [SerializeField] ParticleSystem sucessParticle;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        RespondToDebugKeys();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDisabled)
        {
            return;
        }
        else
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("Starting level!");
                    break;

                case "Finish":
                    Debug.Log("Successfully made it to the landing pad!");
                    LoadingSequence();
                    break;

                default:
                    Debug.Log("You crashed, unlucky!");
                    CrashSequence();
                    break;
            }
        }
    }

    void CrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(collisionSound);
        collisionParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadCurrentLevel", delay);
    }

    void LoadingSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        sucessParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadingNextLevel", delay);
    }

    void ReloadCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadingNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) 
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }


    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadingNextLevel();
        }

        if (Input.GetKey(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
            Debug.Log($"Collision has been {(collisionDisabled ? "disabled" : "enabled")}.");
        }
    }
}
