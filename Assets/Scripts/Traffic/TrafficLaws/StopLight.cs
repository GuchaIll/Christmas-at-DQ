using System.Collections;
using UnityEngine;


[System.Serializable]
public enum StopLightState{
    Red,
    Yellow,
    Green
}
public class StopLight : MonoBehaviour{
    
    public StopLightState currentState = StopLightState.Red;

    [SerializeField] private float redLightDuration = 5f;
    [SerializeField] private float yellowLightDuration = 2f;
    [SerializeField] private float greenLightDuration = 5f;

    private Coroutine stopLightCoroutine;
    private void setRandomState(){
        currentState = (StopLightState)Random.Range(0, 3);
    }

    private void Start()
    {
        setRandomState();
    }


    private IEnumerator CycleThroughtStates()
    {
        while(true)
        {
            currentState = StopLightState.Red;
            Debug.Log("Red Light");
            yield return new WaitForSeconds(redLightDuration);

            currentState = StopLightState.Green;
            Debug.Log("Green Light");
            yield return new WaitForSeconds(greenLightDuration);

            currentState = StopLightState.Yellow;
            Debug.Log("Yellow Light");
            yield return new WaitForSeconds(yellowLightDuration);
        }
    }

    private void OnTriggerEnter(Collider other )
    {
        if(other.CompareTag("Car"))
        {
            Debug.Log("Player entered stop light range");
            stopLightCoroutine = StartCoroutine(CycleThroughtStates());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            Debug.Log("Player exited stop light range");
            if(stopLightCoroutine != null)
            {
                StopCoroutine(stopLightCoroutine);
                stopLightCoroutine = null;

            }
    }
    }
}