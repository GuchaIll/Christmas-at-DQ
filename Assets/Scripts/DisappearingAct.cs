using UnityEngine;

public class DisappearingAct : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private float disappearDistance = 50f; // Distance at which the object disappears

    private MeshRenderer meshRenderer; // Reference to the MeshRenderer component

    void Start()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on the GameObject!");
        }
    }

    void Update()
    {
        // Check the distance to the player
        if (player != null && meshRenderer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Hide the mesh if the player is close enough
            if (distanceToPlayer <= disappearDistance)
            {
                gameObject.SetActive(false); // Hide the mesh
            }
            else
            {
                gameObject.SetActive(true); // Show the mesh
                transform.LookAt(player);
            
         }
    }   
}
}