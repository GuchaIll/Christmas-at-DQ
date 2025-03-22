using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using System;

public class Jellyfish : MonoBehaviour
{
    private double distance = 10000;
    private double threshold = 500;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // distance = player - jellyfishModel
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) { // just for testing darkness
        //if (distance < threshold) {
            // tentacles move toward player
            // darkness (but can see the pedestrian?)
            // scream (need sound asset)
            System.Random rnd = new System.Random();
            double pedChance = rnd.NextDouble();
            if (pedChance < 0.33) {
                // warped pedestrian standing in the road
            }

        }
    }
}
