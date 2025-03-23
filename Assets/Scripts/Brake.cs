using UnityEngine;

public class Brake : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; 
    private bool isBrakeOn = false;
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBrakeOn = !isBrakeOn;
            playerController.toggleParkMode(isBrakeOn);
        }

    }
}