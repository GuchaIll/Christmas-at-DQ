using UnityEngine;

public class SpotLight : MonoBehaviour
{
    private Light spotLight;
    void Start()
    {   
        spotLight = GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            spotLight.enabled = !spotLight.enabled; 
        }
    }
}
