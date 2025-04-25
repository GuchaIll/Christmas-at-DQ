using UnityEngine;

public class WalkerAI : MonoBehaviour
{
    [SerializeField] private Transform pointA; // First point
    [SerializeField] private Transform pointB; // Second point
    [SerializeField] private float speed = 2f; // Walking speed
    [SerializeField] private float terrifiedSpeed = 4f; // Speed when terrified
    [SerializeField] private bool scaredOfLight = false;
    [SerializeField] private bool scaredOfSound = false; 
    [SerializeField] private float scareRadius = 10f; // Distance to be scared of sound
    [SerializeField] private AudioClip terrifiedSound;
    private Transform targetPoint; // Current target point
    private Animator animator; 
    private AudioSource audioSource; 

    private bool terrified = false; // Flag to check if the enemy is terrified
    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Start by moving toward point A
        targetPoint = pointA;

        // Play the walk animation
        if (animator != null)
        {
            Debug.Log("Animator found. Setting isWalking to true.");
            // Assuming "isWalking" is a parameter in the Animator
        }
        else
        {
            Debug.LogError("Animator component not found on the GameObject!");
        }

         audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject!");
        }
    }

    void Update()
    {
        // Move the enemy toward the target point
        if(!terrified)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Check if the enemy has reached the target point
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
            // Switch to the other point
                targetPoint = targetPoint == pointA ? pointB : pointA;
            }

            CheckForScareSources();
        }
       else
        {
            
            animator.SetBool("isRunning", true); // Set the running animation
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, terrifiedSpeed * Time.deltaTime);
        }

        // Log the current position and target
        Debug.Log($"Enemy position: {transform.position}, Target: {targetPoint.position}");
    }

    private void CheckForScareSources()
    {
        // Check for headlight (light scare)
        if (scaredOfLight)
        {
            GameObject headlight = GameObject.FindWithTag("HeadLight");
            if (headlight != null && Vector3.Distance(transform.position, headlight.transform.position) <= scareRadius)
            {
                 SpotLight spotLight = headlight.GetComponent<SpotLight>();
                if (spotLight != null && spotLight.IsSpotLightOn() && Vector3.Distance(transform.position, headlight.transform.position) <= scareRadius)
                {
                    Debug.Log("Walker scared by headlight!");
                   TriggerTerrifiedState();
                    return;
                }
            }
        }

        // Check for radio (sound scare)
        if (scaredOfSound)
        {
            GameObject radio = GameObject.FindWithTag("Radio");
            if (radio != null  && Vector3.Distance(transform.position, radio.transform.position) <= scareRadius)
            {
                Debug.Log("Walker scared by radio!");
                TriggerTerrifiedState();
                return;
            }

             CarHonk carHonk = FindFirstObjectByType<CarHonk>();
            if (carHonk != null && carHonk.IsHonkSoundPlaying() && Vector3.Distance(transform.position, carHonk.transform.position) <= scareRadius)
            {
                Debug.Log("Walker scared by honk!");
                TriggerTerrifiedState();
                return;
            }
        }

    }

     private void TriggerTerrifiedState()
    {
        if (!terrified)
        {
            terrified = true;

            // Play the terrified sound
            if (audioSource != null && terrifiedSound != null)
            {
                audioSource.PlayOneShot(terrifiedSound);
            }
            else
            {
                Debug.LogWarning("Terrified sound or AudioSource is missing!");
            }
        }
    }

    
}

