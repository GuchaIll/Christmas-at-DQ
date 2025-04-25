using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public enum CutSceneNames
{
    Intro,
    Booth,
    MistryForest,
    TurbulentShore,
    Wolfur
    
}


[System.Serializable]
public class CutScene
{
    public CutSceneNames cutSceneName; // Name of the cutscene
    public Sprite[] frames; // Sequence of sprites for this cutscene
}

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private Image cutSceneImage;
    [SerializeField] private CutScene[] cutScenes; // Array of cutscenes
    [SerializeField] private float cutSceneDuration = 2f;

    private int currentCutSceneIndex = 0;
    private int currentFrame = 0;
    private bool isPlaying = false;

    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool loop = false;
    [SerializeField] private bool allowSkip = true;
    private float skipTimer = 0;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject SceneAudio;

    void Start()
    {
        if (playOnStart)
        {
            PlayCutScene();
        }
    }

    void Update()
    {
        // Check for mouse click to skip to the next scene
        if (isPlaying && allowSkip && Input.GetMouseButtonDown(0))
        {
           // SkipCutScene();
        }
    }

    private IEnumerator PlayCutSceneRoutine()
    {
        isPlaying = true;
        currentFrame = 0;
        cutSceneImage.canvas.gameObject.SetActive(true);
        cutSceneImage.gameObject.SetActive(true);
        Cursor.visible = true;

        Sprite[] frames = cutScenes[currentCutSceneIndex].frames;

        while (currentFrame < frames.Length)
       {
         cutSceneImage.sprite = frames[currentFrame];
         float timer = 0f;

        // Wait for the duration of the frame or until the player clicks
         while (timer < cutSceneDuration)
        {
            if (allowSkip && Input.GetMouseButtonDown(0) && currentFrame < frames.Length - 1)
    
            {
                // Skip to the next cutscene
                currentFrame++;
                cutSceneImage.sprite = frames[currentFrame];
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

         currentFrame++;
       }

        isPlaying = false;
        if (loop)
        {
            currentFrame = 0;
            StartCoroutine(PlayCutSceneRoutine());
        }
        else
        {
            ExitCutScene();
            cutSceneImage.gameObject.SetActive(false);
        }
    }

    public void PlayCutScene()
    {
        if (!isPlaying)
        {
            if (playerController != null)
            {
                playerController.toggleParkMode(true);
            }

            StartCoroutine(PlayCutSceneRoutine());
        }
    }

    //public void SkipCutScene()
    //{
        //if (allowSkip)
       // {
       //     // Skip to the next cutscene
           // currentFrame = cutScenes[currentCutSceneIndex].frames.Length;

            // If there are more cutscenes, move to the next one
         //   if (currentCutSceneIndex < cutScenes.Length - 1)
        //    {
         //       currentCutSceneIndex++;
              
       //     }
          
       // }
    //}

  

    public void SetCutScene(CutSceneNames cutSceneName)
    {
        for (int i = 0; i < cutScenes.Length; i++)
        {
            if (cutScenes[i].cutSceneName == cutSceneName)
            {
                currentCutSceneIndex = i;
                return;
            }
        }

        Debug.LogWarning($"Cutscene {cutSceneName} not found!");
    }

    private void ExitCutScene()
    {
        if (playerController != null)
        {
            playerController.toggleParkMode(false);
            SceneAudio.GetComponent<AudioSource>().Play();
            if (cutSceneImage.canvas)
            {
                AudioSource currAudio = cutSceneImage.canvas.GetComponent<AudioSource>();
                if (currAudio != null)
                {
                    currAudio.Stop();
                }
            }
        }
    }
}