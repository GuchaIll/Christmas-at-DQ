using UnityEngine;


//Kills cannot be reversed by elimination the witness
public class killTracker : MonoBehaviour
{
    private int killCounter;
    public delegate void killEventHandler(int newKillCount);
    public event killEventHandler onKillEvent;


    public void AddKill(int kills)
    {
        killCounter += kills;
        onKillEvent?.Invoke(killCounter);
    }

    //subscribe a function to be activated everytime the player kills another beign
    public void subscribeEventToKillCounter(killEventHandler handler)
    {
        onKillEvent += handler;
    }

}