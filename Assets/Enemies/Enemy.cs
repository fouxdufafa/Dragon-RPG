using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 5f;

    AICharacterControl aiCharacterControl = null; // used for setting nav target
    GameObject player = null;

    float currentHealthPoints = 100f;

    bool chasingPlayer = false;

    void Start()
    {
        aiCharacterControl = GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // TODO: make event fired by manager class when player enters radius of enemy
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < attackRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints; 
        }
    }
}
