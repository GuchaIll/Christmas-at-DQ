using System.Collections;
using UnityEngine;



/*This item is triggered when a certain amounts of kills/sacrifices have been made
Once triggered, it boosts the car's performance and active a passive ability
                        -Speed surge
                        -Soul Siphon
                            Absorb a fraction of the enemy's essence upon eliminating them
                            increase by 5% for every kill, 10% risk of corruption, where
                            your karma drains at a fast speed*/

public class BloodRunes : Item
{
    public BloodRunes() : base("BloodRunes")
    {
    }

    private int kills = 0;
    private int sacrfices = 0;
    private bool isTriggered = false;

    private bool triggerRunes = false;

    private bool soulSiphonActivated = false;

    private float cooldown = 10f;

    private float duration = 5f;

    [SerializeField] private int activationCounts = 30;

    void Update()
    {

        //subscribe soul siphon to kill tracking system
        if (kills + sacrfices >= activationCounts && !isTriggered)
        {
            isTriggered = true;
           
        }
    }

    public void AddKill()
    {
        kills++;
        Debug.Log("Kill count: " + kills);
    }

    public void AddSacrifice()
    {
        sacrfices++;
        Debug.Log("Sacrifice count: " + sacrfices);
    }


    //consume kills and sacrifices to trigger the runes

    private IEnumerator TriggerRunes()
    {
        Debug.Log("Blood runes triggered");
        triggerRunes = true;
        speedSurge();
        ToggleSoulSiphon();
        yield return new WaitForSeconds(cooldown);
        triggerRunes = false;
        kills = 0;
        sacrfices = 0;
    }

    //increase the speed of the car
    private void speedSurge()
    {
        Debug.Log("Speed surge activated");
    }

    //passive ability: convert the next kill into karma
    private void ToggleSoulSiphon()
    {
        Debug.Log("Soul siphon activated");
        soulSiphonActivated = !soulSiphonActivated;


    }

    new public void UseItem()
    {
        if(isTriggered)
        {
            TriggerRunes();
        }
    }
}