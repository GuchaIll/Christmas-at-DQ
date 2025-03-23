using UnityEngine;
using System.Collections;


    public class AggresiveDriverAI : BaseDrivingAI
    {
   
    public float overtakingSpeed = 15f;
    public float detectionDistance = 50f;
   
    private bool overtaking = false;

    new public void Update()
    {
        base.Update();

        if(!overtaking)
        {
            DetectAndOvertake();
        }
        
    }
    public void DetectAndOvertake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
        {
            if(hit.collider.CompareTag("Car"))
            {
                overtaking = true;
            }
        }
        else
            {
            overtaking = false;
         }
    }
    
}
    

