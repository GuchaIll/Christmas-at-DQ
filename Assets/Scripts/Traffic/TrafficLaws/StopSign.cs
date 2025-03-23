using System.Collections;
using UnityEngine;

public class StopSign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float stopDuration = 10f;
    private bool playerStopped = false;
    private Coroutine stopCoroutine;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Load the new level
            Debug.Log("Player stopped at stop sign");
        }

        if(stopCoroutine == null)
        {
            stopCoroutine = StartCoroutine(StopSignTimer());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left stop sign");
            if(stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
                stopCoroutine = null;
            }
            if(!playerStopped)
            {
                
            }
            
        }
    }

    private IEnumerator StopSignTimer()
    {
        playerStopped = false;
        yield return new WaitForSeconds(stopDuration);
        playerStopped = true;
        Debug.Log("Player can now leave stop sign");


    }
}