

using UnityEngine;

public class SpeedyBehaviour : EnemyBehaviour
{
    [SerializeField] private Transform target;
    protected override void MovementEnemy()
    {
        agent.SetDestination(target.position);
    }
}
