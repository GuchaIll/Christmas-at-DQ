using UnityEngine;

public class BookCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject book; // Reference to the book GameObject

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {

            book.SetActive(false);

             CameraRotation cameraController = FindFirstObjectByType<CameraRotation>();

            if (cameraController != null)
            {
                cameraController.enableCameraRotation();
        }

        }
    }
}