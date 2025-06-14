using System;
using UnityEngine;
public class InkyBehaivour : EnemyBehaviour
{
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform blinky;
    [SerializeField] private Transform inkyTarget;
    [SerializeField] private int amountToStart;
    
    private float distance;

    private void OnEnable()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    protected override void MovementEnemy()
    {
        distance = Vector3.Distance(blinky.position, targetPlayer.position);
        inkyTarget.localPosition = new Vector3(distance / 2f,0,distance / 1.5f);
        agent.SetDestination(inkyTarget.position);
        
    }

    protected override bool CanAct()
    {
        return gameManager.coinsPicked > amountToStart;
    }
}
