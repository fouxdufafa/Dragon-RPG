using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 5f;
    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float moveRadius = 7f;
    [SerializeField] float secondsBetweenShots = .5f;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSocket;

    AICharacterControl aiCharacterControl = null; // used for setting nav target
    GameObject player = null;

    float currentHealthPoints;

    bool isFiring = false;

    void Start()
    {
        aiCharacterControl = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealthPoints = maxHealthPoints;
    }

    private void Update()
    {
        // TODO: make event fired by manager class when player enters radius of enemy
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < moveRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            // Stop
            aiCharacterControl.SetTarget(transform);
        }

        if (distanceToPlayer < attackRadius && !isFiring)
        {
            isFiring = true;
            InvokeRepeating("SpawnProjectile", 0, secondsBetweenShots);
        }
        
        if (distanceToPlayer > attackRadius && isFiring)
        {
            isFiring = false;
            CancelInvoke("SpawnProjectile");
        }
    }

    void SpawnProjectile()
    {
        GameObject bullet = Instantiate(projectile, projectileSocket.transform, false);
        Projectile projectileComponent = bullet.GetComponent<Projectile>();
        projectileComponent.direction = (player.transform.position + aimOffset) - bullet.transform.position;
        projectileComponent.damageAmount = damagePerShot;
    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints; 
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - amount, 0f, maxHealthPoints);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(projectileSocket.transform.position, 0.2f);
    }
}
