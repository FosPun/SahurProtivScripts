using System;
using UnityEngine;

public class PowerPickUp : Collectable
{
    
    protected override void OnCollect()
    {
        foreach (EnemyBehaviour enemy in enemyBehaviour)
        {
            enemy.isWeak = true;
            enemy.timerWeakness = 0;
        }
    }
}
