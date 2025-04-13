using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] GameObject entityPrefab;
    public Vector3 entityFacing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityFacing = GetComponentInChildren<Transform>().position.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        entityFacing = GetComponentInChildren<Transform>().position.normalized;
    }
}
