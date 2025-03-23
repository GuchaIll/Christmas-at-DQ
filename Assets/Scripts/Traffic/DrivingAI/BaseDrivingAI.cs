using UnityEngine;
using System.Collections;

public class BaseDrivingAI : MonoBehaviour
{
     public Transform[] waypoints;
    public float speed = 10f;
    public float turnSpeed = 5f;
     private int currentWaypointIndex = 0;
    public void Update()
    {
        if(waypoints.Length == 0)
        {
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        //float currentSpeed = overtaking ? overtakingSpeed : speed;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

    }
}
