using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTriggerVolume : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
       
            // Load the new level
            SceneManager.LoadScene("Level1");
            Debug.Log("Level Loaded");
        
    }
}
