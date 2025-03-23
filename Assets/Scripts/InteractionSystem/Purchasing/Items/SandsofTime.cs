using UnityEngine;

/*This item reverses any loss to karma or increase to bloolust
by the player for the past 10 seconds
should be used when player suffers substantial loss 8? */


public class SandsofTime : MonoBehaviour
{
    [SerializeField] private float duration;
    private PlayerController player;
    private InventorySystem inventorySystem;
    private killTracker killTracker; //update to killTracker

    public void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        inventorySystem = player.GetComponent<InventorySystem>();
        killTracker = player.GetComponent<killTracker>();


    }

    //remove past instances of wrong from the current karma
    public void UseItem()
    {
        
    }
}