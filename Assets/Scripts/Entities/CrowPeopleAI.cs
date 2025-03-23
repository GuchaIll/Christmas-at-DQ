using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[System.Serializable]
public enum CPState
{
    Stationary,
    Patrol,
    SightedPlayer, 
    Angry,
    Trading,
    Dead
}

public class CrowPeopeAI : MonoBehaviour
{
    
    public CPState currentState = CPState.Stationary;
    [SerializeField] private string[] StationaryDialogueLines;
    [SerializeField] private string[] TradingDialogueLines;
    [SerializeField] private string[] AngryDialogueLines;
    [SerializeField] private string[] DeadDialogueLines;

    private string characterName = "Crow People";
    private DialogueSystem dialogueSystem;
    private bool isInteracting = false;




    //Audio Source
    private AudioSource audioSource;
    [SerializeField] private AudioClip TradingAudioClip;
    [SerializeField] private AudioClip SuccessfulTradeAudioClip;
    [SerializeField] private AudioClip AngryScreamAudioClip;
    [SerializeField] private AudioClip [] MumbleAudioClip;
    [SerializeField] private AudioClip idleAudioClip;

    MarketSystem marketSystem;

    //State changes
    [SerializeField] private float angryDuration = 5f;
    [SerializeField] private bool isStationary;
    [SerializeField] private bool isTradable;

    [SerializeField] private Transform patrolPointA;
    [SerializeField] private Transform patrolPointB;
    private Transform currentPatrolTarget;
    [SerializeField] private float patrolSpeed = 5f;
    [SerializeField] private float normalPatrolSpeed = 5f;

    private NavMeshAgent navMeshAgent;


    void Awake()
    {
        dialogueSystem = FindFirstObjectByType<DialogueSystem>();
        audioSource = GetComponent<AudioSource>();
        marketSystem =  GetComponent<MarketSystem>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if(audioSource != null)
        {
            audioSource.clip = idleAudioClip;
            audioSource.loop = true;
        }

        currentPatrolTarget = patrolPointA;
    
    }

    void Update()
    {
       switch(currentState)
        {
            case CPState.Stationary:
                StationaryState();
                break;
            case CPState.Patrol:
                PatrolState();
                break;
            case CPState.SightedPlayer:
                SightedPlayerState();
                break;
            case CPState.Angry:
                AngryState();
                break;
            case CPState.Dead:
                DeadState();
                break;
            case CPState.Trading:
                TradingState();
                break;
    }
    }

    void updateState(CPState newState)
    {
        currentState = newState;
        isInteracting = false;
        
         
    }

        void StationaryState()
        {
            if(!isInteracting)
            {
                isInteracting = true;
                dialogueSystem.InitiateNewDialogue(characterName, StationaryDialogueLines);
            }
        }

        //Patrols back and forth
        void PatrolState()
        {
            if(Vector3.Distance(transform.position, currentPatrolTarget.position) < 0.5f)
            {
                currentPatrolTarget = currentPatrolTarget == patrolPointA ? patrolPointB : patrolPointA;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentPatrolTarget.position, patrolSpeed * Time.deltaTime);
        }

        //Increased movement speed
        void AngryState()
        {
            if(!isInteracting)
            {
                isInteracting = true;
                dialogueSystem.InitiateNewDialogue(characterName, AngryDialogueLines);
            }

            patrolSpeed *= 2;
            Invoke("RevertSpeed", angryDuration);
            updateState(CPState.Patrol);
        
        }

        void RevertSpeed()
        {
            patrolSpeed = normalPatrolSpeed;
            
        }

        //Rag doll, disable physics
        void DeadState()
        {
            this.enabled = false;
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }

            Collider mainCollider = GetComponent<Collider>();
            if (mainCollider != null)
            {
                mainCollider.enabled = false;
            }

            Rigidbody mainRigidbody = GetComponent<Rigidbody>();
            if (mainRigidbody != null)
            {
                mainRigidbody.isKinematic = true;
            }

        }

        //Initiates dialogue and trading, create trading menu
        void TradingState()
        {
            if(!isInteracting)
            {
                 isInteracting = true;
                dialogueSystem.InitiateNewDialogue(characterName, TradingDialogueLines);
            }
          
        }

        Transform QueryForPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
            {
                return player.transform;
            }
            return null;
        }

        //Detects player, initiates chase
        void SightedPlayerState()
        {
            Transform playerLocation = QueryForPlayer();
            if(playerLocation != null)
            {
                navMeshAgent.SetDestination(QueryForPlayer().position);
            }
        }
    

}




