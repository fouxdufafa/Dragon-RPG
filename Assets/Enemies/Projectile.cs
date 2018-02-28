using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float speed = 10f;

    public Vector3 direction = Vector3.zero;
    public float damageAmount = 10f;

    private void Start()
    {
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageAmount);
        }
    }
}
