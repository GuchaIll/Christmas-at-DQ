using UnityEngine;

public class SpotLight : MonoBehaviour
{
    private Light spotLight;
    private Light highBeam;
    void Start()
    {   
        spotLight = GetComponent<Light>();
        highBeam = GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (spotLight.enabled) {
                highBeam.enabled = !highBeam.enabled;
            } else if (highBeam.enabled) {
                spotLight.enabled = !spotLight.enabled;
                highBeam.enabled = !highBeam.enabled;
            } else {
                spotLight.enabled = !spotLight.enabled; 
            }
            
        }
    }
}
