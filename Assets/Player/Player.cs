using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }
    float currentHealthPoints;

    [SerializeField] float attackPower = 30f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float minTimeBetweenAttacks = 0.5f;
    float lastAttackTime;

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    GameObject currentTarget = null;

    private void Start()
    {
        CameraRaycaster raycaster = Camera.main.GetComponent<CameraRaycaster>();
        raycaster.notifyMouseClickObservers += OnMouseClick;
        currentHealthPoints = maxHealthPoints;
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        switch (layerHit)
        {
            case walkableLayerNumber:
                currentTarget = null;
                break;

            case enemyLayerNumber:
                GameObject enemy = raycastHit.collider.gameObject;
                if ((enemy.transform.position - transform.position).magnitude > attackRange)
                {
                    // Don't attack if out of range
                    return;
                }
                currentTarget = enemy;

                if (Time.time - lastAttackTime > minTimeBetweenAttacks)
                {
                    (enemy.GetComponent<IDamageable>()).TakeDamage(attackPower);
                    lastAttackTime = Time.time;
                }
                
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - amount, 0f, maxHealthPoints);
    }
}
