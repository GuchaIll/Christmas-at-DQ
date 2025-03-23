using UnityEngine;

public class SpotLight : MonoBehaviour
{
    private Light spotLight;
    void Start()
    {   
        spotLight = GetComponent<Light>();
        spotLight.enabled = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            spotLight.enabled = !spotLight.enabled; 
        }
    }
}
