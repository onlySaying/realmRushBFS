using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float rangae = 15f;
    Transform target;

    void Update()
    {
        FindClosedTarget();
        AimWeapon();
    }

    void FindClosedTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies) 
        {
            float targetDistanve = Vector3.Distance(transform.position, enemy.transform.position);
            if(targetDistanve < maxDistance) 
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistanve;
            }
        }

        target = closestTarget;
    }

    void AimWeapon()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);
        weapon.LookAt(target);
        if(targetDistance < rangae)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
