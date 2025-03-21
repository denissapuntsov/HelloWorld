using StarterAssets;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Footsteps : MonoBehaviour
{
    [SerializeField] float footstepOffset;

    Vector3 lastPosition;
    Animator footstepAnimator;
    AudioSource footAudioSource;
    FirstPersonController controller;
    float playerStartingSpeed;
    [SerializeField] float stepThreshold = 0.01f;
    [SerializeField] AudioClip[] rugFootstepClips, tileFootstepClips, concreteFootstepClips, stairsFootstepClips;
    public AudioClip[] activeClipPool;
    public bool isMoving = false;
    

    private void Start()
    {
        footAudioSource = GetComponent<AudioSource>();
        footstepAnimator = GetComponent<Animator>();
        controller = FindAnyObjectByType<FirstPersonController>();
        playerStartingSpeed = controller.MoveSpeed;
        lastPosition = transform.position;
        SetActiveClipPool(0);
    }

    private void Update()
    {
        CheckForMovement();
    }

    private void CheckForMovement()
    {
        // checks the distance of movement between the last frame, disregards negligible distance
        isMoving = Mathf.Abs(lastPosition.x - transform.position.x) > stepThreshold || Mathf.Abs(lastPosition.z - transform.position.z) > stepThreshold; 
        footstepAnimator.SetBool("isMoving", isMoving);
        lastPosition = transform.position;

        // sets length of the next step cycle based on the speed of the player
        footstepAnimator.speed = controller.MoveSpeed / playerStartingSpeed;
    }

    public void SetActiveClipPool(int poolIndex)
    {
        switch (poolIndex)
        {
            case 0:
                activeClipPool = rugFootstepClips;
                Debug.Log("on floor");
                break;
            case 1:
                activeClipPool = tileFootstepClips;
                Debug.Log("on rug");
                break;
            case 2:
                activeClipPool = concreteFootstepClips;
                Debug.Log("on tile");
                break;
            case 3:
                activeClipPool = stairsFootstepClips;
                Debug.Log("on stairs");
                break;
        }
    }

    public void playStep()
    {
        AudioClip nextStepClip = activeClipPool[Random.Range(0, activeClipPool.Length)];
        footAudioSource.pitch = Random.Range(0.95f, 1.05f);
        footAudioSource.PlayOneShot(nextStepClip);
    }
}
