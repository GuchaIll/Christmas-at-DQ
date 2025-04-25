using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the new level here
            // For example, using SceneManager.LoadScene("NewLevelName");
            Debug.Log("Loading new level...");
            SceneManager.LoadScene("TurbulentShore");
        }
    }
}
