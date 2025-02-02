using System.Collections.Generic;

public class Subject
{
    private List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer)
    {
        if(!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void UnregisterObserver(IObserver observer)
    {
       if(!observers.Contains(observer))
       {
           observers.Remove(observer);
       }
    }

    public void NotifyObservers(string eventMessage)
    {
        foreach (var observer in observers)
        {
            //observer.OnNotify(eventMessage);
        }
    }
}