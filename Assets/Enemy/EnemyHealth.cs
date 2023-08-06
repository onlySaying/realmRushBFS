using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    [Tooltip("adds amount to maxhitpoint when enemy dies")]
    [SerializeField] int difficutyRamp = 1;
    int currentHitPoints = 0;
    Enemy enemy;
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }
    private void Start()
    {
        enemy = GetComponent<Enemy>(); 
    }
    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints--;
        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficutyRamp;
            enemy.RewardGold();
        }    
    }
}
