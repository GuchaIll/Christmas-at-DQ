using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private PlayerController playerController;
   
    
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        if(playerController != null)
        {
            playerController.transform.position = transform.position;
            Debug.Log("Player Start Position Set");
        }
        else{
            Debug.LogWarning("Player Controller not found");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
