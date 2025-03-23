using System;
using UnityEngine;

public class BookController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject book;

    void Start()
    {
        if(book != null)
        {
            book.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if(book != null)
        {
            Debug.Log("Book is active");
            book.SetActive(!book.activeSelf);
        }
    }

      void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Draw the ray in the Scene view for debugging
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

            int layermask = LayerMask.GetMask("PostProcessing");
            if ( Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layermask))
            {
                // Check if the ray hit this GameObject
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Book clicked via Raycast!");
                    if (book != null)
                    {
                        book.SetActive(true); // Toggle the book's active state
                     
                    }
                }
              
            }
        }
    }
}
