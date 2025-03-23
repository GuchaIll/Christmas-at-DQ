using UnityEngine;


public class Item : MonoBehaviour
{
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescription;
    [SerializeField] public int itemPrice;
    [SerializeField] public Sprite itemSprite;

    [SerializeField] public bool isStackable;

    [SerializeField] public bool isConsumable;
    [SerializeField] public int maxStack;

    private InventorySystem inventorySystem;

    //overridable function, what should happen when the item is used

    public Item(string itemName){}
    public void UseItem()
    {
        
    }



}