using UnityEngine;
using System.Collections;

public class JaywalkingDeerAI : MonoBehaviour
{
    [SerializeField] private float scareDistance = 10f; // Distance at which the deer gets triggered
    [SerializeField] private float runSpeed = 10f; // Speed at which the deer runs
    [SerializeField] private Transform runDirection; // Target position for the deer to run to
    [SerializeField] private AudioClip scareSound; // Optional sound for the jump scare

    private bool hasScaredPlayer = false; // Ensures the scare happens only once
    private AudioSource audioSource; // For playing the scare sound

    void Start()
    {
        // Initialize the audio source if a scare sound is provided
        if (scareSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = scareSound;
        }
    }

    void Update()
    {
        // Check if the player is within scare distance
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && !hasScaredPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= scareDistance)
            {
                TriggerJumpScare(player);
            }
        }
    }

    private void TriggerJumpScare(GameObject player)
    {
        hasScaredPlayer = true; // Ensure the scare happens only once

        // Play the scare sound
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Start running across the path
        StartCoroutine(RunAcrossPath());

        Debug.Log("Deer jump scare triggered!");
    }

    private IEnumerator RunAcrossPath()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = runDirection.position;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
            yield return null;
        }

        // Optionally, disable the deer after it finishes running
        gameObject.SetActive(false);
    }
}