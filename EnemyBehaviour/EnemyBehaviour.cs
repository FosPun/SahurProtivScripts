using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;
using YG;

public abstract class EnemyBehaviour : MonoBehaviour
{
    private enum State
    {
        PATROLING_STATE ,
        SCATERRING_STATE ,
        DEAD_STATE , 
        WEAKNESS_STATE
    }
    
    //Reference
    protected GameManager gameManager;
    protected Transform targetPlayer;
    protected NavMeshAgent agent;
    private ScoreCount scoreCount;
    private MeshRenderer model;
    private ParticleSystem particle;
    //Weakness State
    [SerializeField] public bool isWeak = false;
    public float timerWeakness;
    [SerializeField] private float durationWeakness;
    //Respawn State
    [SerializeField] private float speedRespawning = 12;
    [SerializeField] private bool isDead;
    public Vector3 startPosition;
    private float startSpeed;
    //Chtoto
    [SerializeField] private int amount;
    [SerializeField] private float timerForScatter;
    private float timer;
    Random random;
    [SerializeField] private Vector3 randomTarget;
    private Coroutine blinkingCoroutine;
    [SerializeField] private State currentState;
    AudioSource memeSound;
    AudioSource[] playerSound;
    [SerializeField] private float speedInWeaknessState = 2f;
    private void Start()
    {
        currentState = State.PATROLING_STATE;
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindFirstObjectByType<GameManager>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        agent.speed += gameManager.speedLevelUpgrade * (YG2.saves.level);
        startSpeed = agent.speed;
        model = this.gameObject.GetComponentInChildren<MeshRenderer>();
        particle = this.gameObject.GetComponentInChildren<ParticleSystem>();
        random = new Random();
        memeSound = GetComponent<AudioSource>();
        playerSound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
    }

    private void Update()
    {
        if(!CanAct()) return;
        timer += Time.deltaTime;

        if (timer >= timerForScatter && !isWeak && !isDead)
        {
            currentState = State.SCATERRING_STATE;
        }
        else if (isWeak && !isDead)
        {
            currentState = State.WEAKNESS_STATE;
            
            if (Vector3.Distance(transform.position, randomTarget) < 1)
            {
                GenerateNewRandomTarget();
            }
            
            blinkingCoroutine ??= StartCoroutine(Blinking());
        }
        switch (currentState)
        {
            case State.PATROLING_STATE:
                RandomMovement();
                break;
            case State.SCATERRING_STATE: 
                MovementEnemy();
                break;
            case State.DEAD_STATE:
                StopAllCoroutines();
                RespawnGhost();
                break;
            case State.WEAKNESS_STATE:
                agent.speed = speedInWeaknessState;
                RandomMovement();
                break;
        }
        
        WeaknessTimer();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isWeak)
        {
            KillGhost();
        }
        else
        {
            gameManager.DamagePlayer();
            memeSound.Play();
        }
    }

    private void KillGhost()
    {   if(isDead) return;
        scoreCount = FindFirstObjectByType<ScoreCount>();
        scoreCount.AddScore(amount);
        model.enabled = false;
        particle.Play();
        isDead = true;
        agent.speed = speedRespawning;
        currentState = State.DEAD_STATE;
        GenerateNewRandomTarget();
        playerSound[0].Play();
    }
    
    private void RespawnGhost()
    {
        if (!isDead) return;

        agent.SetDestination(startPosition);
        if (!(Vector3.Distance(gameObject.transform.position, startPosition) < 1)) return;

        isDead = false;
        isWeak = false;
        model.enabled = true;
        particle.Stop();
        agent.speed = startSpeed;
        currentState = State.SCATERRING_STATE;
        blinkingCoroutine = null;
        
        GenerateNewRandomTarget(); 
    }

    private void WeaknessTimer()
    {
        if (!isWeak) return;
        timerWeakness += Time.deltaTime;
        if (timerWeakness >= durationWeakness)
        {
            timerWeakness = 0;
            isWeak = false;

            // Останавливаем мигание
            if (blinkingCoroutine != null)
            {
                StopCoroutine(blinkingCoroutine);
                blinkingCoroutine = null;
            }

            model.enabled = true; // Убедиться, что включен
        }
    }


    private void RandomMovement()
    {
        if (Vector3.Distance(transform.position, randomTarget) < 1)
        {
            GenerateNewRandomTarget();
        }
        agent.SetDestination(randomTarget);

    }

    private void GenerateNewRandomTarget()
    {
        while (true)
        {
            Vector3 randomPos = new Vector3(random.Next(-13, 13), 0.5f, random.Next(-12, 16));

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 2.0f, NavMesh.AllAreas))
            {
                randomTarget = hit.position;
            }
            else
            {
                continue;
            }

            break;
        }
    }


    public void GoToStartPosition()
    {
        agent.enabled = false;
        transform.position = startPosition;
        agent.enabled = true;

        if (CanAct())
        {
            GenerateNewRandomTarget(); // Чтобы снова начал двигаться
            agent.SetDestination(randomTarget);
        }
    }
    

    private IEnumerator Blinking()
    {
        while (isWeak)
        {
            model.enabled = !model.enabled;
            yield return new WaitForSeconds(1f); // мигаем каждую секунду
        }

        model.enabled = true;
    }
    protected abstract void MovementEnemy();

    protected virtual bool CanAct()
    {
        return true;
    }
}
