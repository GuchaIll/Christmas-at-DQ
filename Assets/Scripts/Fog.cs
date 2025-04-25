using UnityEngine;
using UnityEngine.SceneManagement;

public class Fog : MonoBehaviour
{
    public GameObject player;
    public float killDistance = 5f;

    [SerializeField] private GameObject menu; // Reference to the menu GameObject
    [SerializeField] private float delayBeforeReset = 2f; // Delay before resetting the scene

    private bool isResetting = false; // Tracks whether the reset process has started
    private float resetTimer = 0f; // Timer to track the delay

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the kill volume
        if (other.gameObject == player && !isResetting)
        {
            Debug.Log("Player entered kill volume. Resetting level and displaying menu.");
            StartResetProcess();
        }
    }

    private void StartResetProcess()
    {
        // Display the menu
        if (menu != null)
        {
            menu.SetActive(true);
            Debug.Log("Menu displayed.");
        }
        else
        {
            Debug.LogWarning("Menu GameObject is not assigned!");
        }

        // Start the reset timer
        isResetting = true;
        resetTimer = delayBeforeReset;
    }

    private void Update()
    {
        // Handle the delay timer
        if (isResetting)
        {
            resetTimer -= Time.deltaTime;

            if (resetTimer <= 0f)
            {
                ResetLevel();
            }
        }
    }

    private void ResetLevel()
    {
        // Reload the current scene
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Reset the state
        isResetting = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Deactivate the menu after the scene is loaded
        if (menu != null)
        {
            menu.SetActive(false);
            Debug.Log("Menu deactivated after scene reload.");
        }

        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}