using UnityEngine;


[System.Serializable]
public struct InventoryItem
{
    public Item item;
    public int quantity;

    public InventoryItem(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
};
public class InventorySystem : MonoBehaviour
{


    [SerializeField] private InventoryItem[] items;
    [SerializeField] private int maxSlot;
    [SerializeField] public int karma;

    //test adding item
    private void Start()
    {
        Item testItem = new Item("Apple");
        AddItem(new InventoryItem(testItem, 1));
    }

    public bool AddItem(InventoryItem newItem)
    {
        
        //check if the newItem is already into the inventory system
        //check if item is stackable and the max stack has not been reached
        //if it is, add the quantity to the existing item
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == newItem.item && items[i].item.isStackable && items[i].quantity < items[i].item.maxStack)
            {
                items[i].quantity += newItem.quantity;
                return true;
            }
        }
     
        //cannot add new item, inventory full
        if(items.Length >= maxSlot) return false;
        //Add new item into inventory
        for(int i = 0; i < items.Length; i++)
        {
               
            
            if(items[i].item == null)
            {
                items[i] = newItem;
            }
            
            
        }
        return false;
    }

    public bool AddItem(Item newItem)
    {
        
        //check if the newItem is already into the inventory system
        //check if item is stackable and the max stack has not been reached
        //if it is, add the quantity to the existing item
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].item == newItem && items[i].item.isStackable && items[i].quantity < items[i].item.maxStack)
            {
                items[i].quantity += 1;
                return true;
            }
        }

        //Add new item into inventory
        for(int i = 0; i < items.Length; i++)
        {
             
            if(items[i].item == null)
            {
                items[i] = new InventoryItem(newItem, 1);
                 return true;
            }
            
            
        }
        return false;
    }
        public bool RemoveItem(InventoryItem itemToRemove)
        {
            for(int i = 0; i < items.Length; i++)
            {
                if(items[i].item == itemToRemove.item)
                {
                    items[i].quantity -= itemToRemove.quantity;
                    if(items[i].quantity <= 0)
                    {
                        items[i] = new InventoryItem(null, 0);
                    }
                    return true;
                }
            }
            return false;

        }

        public bool ContainsItem(InventoryItem itemToCheck)
        {
            for(int i = 0; i < items.Length; i++)
            {
                if(items[i].item == itemToCheck.item)
                {
                    return true;
                }
            }
            return false;
        }
    

    public void UseItem(InventoryItem usedItem)
    {
        if(ContainsItem(usedItem))
        {
            usedItem.item.UseItem();
            RemoveItem(usedItem);
            Debug.Log("Item not found in inventory");
        }
        else{
            Debug.Log("Item not found in inventory");
        }
    }   

    public bool ApplyCost(int cost)
    {
        if(cost > karma)
        {
            Debug.Log("Error: Cost greater than karma");
            return false;
        }
       
        karma -= cost;
        return true;
    }

    //return false if the purchase cannot be made
    public bool CheckCost(int cost)
    {
        return cost >= karma ? false : true;
    }
   
}