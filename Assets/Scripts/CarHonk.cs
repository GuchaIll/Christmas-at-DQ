using UnityEngine;

public class CarHonk : MonoBehaviour
{
    private AudioSource honkSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        honkSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            honkSound.Play();
        }
    }
}
