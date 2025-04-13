using System;
using Unity.Mathematics;
using UnityEngine;

public class JaywalkingDeer2 : MonoBehaviour
{
    public enum Mode
    {
        Waiting,
        Dashing,
        Frozen,
        Retreating,
    };

    public Mode mode = Mode.Waiting;
    Vector3 velocity = Vector3.zero;

    [SerializeField] private float scareTime = 1.6f; // If the player is moving faster, the deer should be triggered farther away.
    [SerializeField] private float scareDistance = 10f; // Distance at which the deer gets triggered
    [SerializeField] private float freezeDistance = 10f; // Distance from the path of the player at which the deer freezes.
    [SerializeField] private float runSpeed = 10f; // Speed at which the deer runs
    [SerializeField] private Transform runDirection; // Target position for the deer to run to
    [SerializeField] private AudioClip scareSound; // Optional sound for the jump scare

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    bool deerProxim(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
    {
        float t_top = (start1.x - start2.x) * (start2.z - end2.z) - (start1.z - start2.z) * (start2.x - end2.x);
        float u_top = (start1.x - end1.x) * (start1.z - start2.z) - (start1.z - end1.z) * (start1.x - start2.x);
        float denom = (start1.x - end1.x) * (start2.z - end2.z) - (start1.z - end1.z) * (start2.x - end2.x);

        float t = t_top / denom;
        float u = u_top / denom;

        float dist1 = (start2 - start1).magnitude;
        float dist2 = (start2 - end1).magnitude;
        float dist3 = (end1 - start1).magnitude;

        //Calculate distance from deer to oncoming traffic.
        float s = 0.5f * (dist1 + dist2 + dist3);
        float deer_dist_from_traffic = 2.0f * math.sqrt(s * (s - dist1) * (s - dist2) * (s - dist3)) / dist3;

        return 0f <= t && t <= 1f && 0f <= u && u <= 0.5f && deer_dist_from_traffic <= freezeDistance;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 projected_position = player.transform.position + player.GetComponent<PlayerController>().velocity * scareTime;

        Mode init_mode = mode;

        if (mode == Mode.Retreating || mode == Mode.Dashing)
        {
            velocity = (runDirection.position - transform.position).normalized * runSpeed * (mode == Mode.Retreating ? 1.15f : 1.0f);
        }

        if (mode == Mode.Waiting)
        {
            if ((projected_position - transform.position).magnitude <= scareDistance)
            {
                mode = Mode.Dashing;
            }
        }
        else if (mode == Mode.Dashing)
        {
            transform.position += velocity * Time.deltaTime;
            if (deerProxim(player.transform.position, projected_position, transform.position, runDirection.position))
            {
                mode = Mode.Frozen;
            }
        }
        else if (mode == Mode.Frozen) {
            velocity = Vector3.zero;
            if (player.GetComponent<PlayerController>().velocity.magnitude < 0.5f) {
                mode = Mode.Retreating;
            }
        }
        else if (mode == Mode.Retreating)
        {
            transform.position += velocity * Time.deltaTime;
        }

        if (mode != init_mode)
        {
            Debug.Log("Mode changed to: " + mode.ToString());
        }
    }
}
