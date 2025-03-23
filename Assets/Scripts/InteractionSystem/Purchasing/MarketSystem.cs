using UnityEngine;

[System.Serializable]
public struct MarketItem
{
    public Item item;
    public int cost;
    public bool purchased; //one one instance of an item can be purchased each time
}

class MarketSystem : MonoBehaviour
{
    
    private InventorySystem inventorySystem;
    [SerializeField] private MarketItem[] purchasableItems;
    

    public void Start()
    {
        inventorySystem = GetComponent<InventorySystem>();

    }

    public bool PurchaseItem(MarketItem desiredItem)
    {
        if(inventorySystem)
        {
            if(inventorySystem.CheckCost(desiredItem.cost))
            {
                Debug.Log("Purchased Item", desiredItem.item);
            
                if(inventorySystem.AddItem(desiredItem.item))
                {
                     inventorySystem.ApplyCost(desiredItem.cost);
                     desiredItem.purchased = true;
                     return true;
                }
                Debug.Log("Inventory Full, cannot purchase item");
            
            }
            else{
                Debug.Log("Purchase cannot be made, insufficient funds");
            }
        }
        else{
            Debug.Log("Error inventory System component not found");
        }
         return false;
    }
   
    

    
}