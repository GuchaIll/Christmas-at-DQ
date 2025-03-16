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
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            spotLight.enabled = !spotLight.enabled; 
        }
    }
}
