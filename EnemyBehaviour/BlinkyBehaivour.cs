
public class BlinkyBehaivour : EnemyBehaviour
{
    protected override void MovementEnemy()
    {
        agent.SetDestination(targetPlayer.position);
    }
}
