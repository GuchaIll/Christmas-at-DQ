using UnityEngine;
using System.Collections;

/*This item when equipped grants a brief window of stealth,
hides the player from the witnesses
This is effective against walkers and
also can be used to hide kills*/

public class RavensFeatherCharms : MonoBehaviour
{
    
    //Get Player reference to set the visibility status on triggered

    private PlayerController player;
    [SerializeField] float duration;
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

     IEnumerator DurationTimer()
    {
        player.setVisibility(true);
        yield return new WaitForSeconds(duration);
        player.setVisibility(false);
    }
    void UseIem()
    {   
        if(player){
            
            StartCoroutine(DurationTimer());
        }
        else{
            Debug.Log("Error: Player reference not found");
        }

    
    }
}