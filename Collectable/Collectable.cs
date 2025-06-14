using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private ScoreCount scoreCount;
    protected EnemyBehaviour[] enemyBehaviour;
    private GameManager gameManager;
    [SerializeField] private int amountScore;


    private void Start()
    {
        scoreCount = FindFirstObjectByType<ScoreCount>();
        enemyBehaviour = FindObjectsByType<EnemyBehaviour>(FindObjectsSortMode.None);
        gameManager = FindFirstObjectByType<GameManager>();
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        gameManager.coinsPicked++;
        scoreCount.AddScore(amountScore);
        OnCollect();
        Destroy(gameObject);

    }
    
    protected abstract void OnCollect();
}
