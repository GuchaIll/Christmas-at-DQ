using UnityEngine;


[System.Serializable]
public enum State
{
    Passive, 
    Angry,
    Crying,
    Inverted
}
public class WomanInTheFogAI : MonoBehaviour, InteractionInterface, LineofSightInterface
{
    public State currentState = State.Passive;
    [SerializeField] private string[] RoadDialogueLines;
    [SerializeField] private string[] AngryDialogueLines;
    [SerializeField] private string[] CarDialogueLines;
    [SerializeField] private string[] InvertedDialogueLines;

    private string characterName = "Woman in the Fog";
    private DialogueSystem dialogueSystem;

    private bool isInteracting = false;

    private bool isFaceInverted = false;

    [SerializeField] GameObject face;
    private float sightedTimer = 0f;
    [SerializeField] private float sightedTimerMax = 5f;

    private bool isInCar = false;

    private AudioSource audioSource;
    [SerializeField] private AudioClip cryingAudioClip;
    [SerializeField] private AudioClip NeckSnapAudioClip;
    [SerializeField] private AudioClip PassiveAudioClip;
    [SerializeField] private AudioClip AngryScreamAudioClip;
    void Awake()
    {
        dialogueSystem = FindFirstObjectByType<DialogueSystem>();
        audioSource = GetComponent<AudioSource>();

        if(audioSource != null)
        {
            audioSource.clip = PassiveAudioClip;
            audioSource.loop = true;
        }
        

    }
    void Update()
    {
        switch(currentState)
        {
            case State.Passive:
                PassiveState();
                break;
            case State.Angry:
                AngryState();
                break;
            case State.Crying:
                CryingState();
                break;
            case State.Inverted:
                InvertedState();
                break;
            
        }

       
    }

    public void Interact()
    {
        if(isInteracting)
        {
            return;
        }
        isInteracting = true;
       switch(currentState)
       {
            case State.Passive:
                dialogueSystem.InitiateNewDialogue(characterName, RoadDialogueLines);
                break;
            case State.Angry:
                dialogueSystem.InitiateNewDialogue(characterName, AngryDialogueLines);
                break;
            case State.Crying:
                dialogueSystem.InitiateNewDialogue(characterName, CarDialogueLines);
                break;
            case State.Inverted:
                dialogueSystem.InitiateNewDialogue(characterName, InvertedDialogueLines);
                break;
                
       }

         
    }

    public void OnLineOfSightEnter()
    {
        Debug.Log("Line of Sight Enter");
        if(currentState != State.Inverted)
        {
            return;
        }
        sightedTimer += Time.deltaTime;
        if(sightedTimer >= sightedTimerMax)
        {
            currentState = State.Inverted;
        }
    }

    public void OnLineOfSightExit()
    {
        Debug.Log("Line of Sight Exit");
        if(currentState == State.Angry)
        {
            return;
        }
        sightedTimer = 0f;
    }

    void PassiveState()
    {
        Debug.Log("Passive");
    }

    void AngryState()
    {
        //Attacks the player
        Debug.Log("Angry");
        if(isInCar)
        {
            //Play Attack Animations
        }
        else{
            //path find towards player
        }

    }

    void CryingState()
    {
        Debug.Log("Crying");
        if(audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = cryingAudioClip;
            audioSource.Play();
        }
    }

    void InvertedState()
    {
        if(isFaceInverted)
        {
            return;
        }

        // Invert the face
        audioSource.clip = NeckSnapAudioClip;
        audioSource.Play();

        face.transform.Rotate(0, 180, 0);
        isFaceInverted = true;

        Debug.Log("Inverted");
    }


}