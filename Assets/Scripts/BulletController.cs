using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidRigidbody2D;

    public void MyStart(float speed)
    {
        IEnumerator Do()
        {
            myRigidRigidbody2D.AddForce(Vector2.up * speed);
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        StartCoroutine(Do());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag($"Enemy"))
        {
            var enemy = col.transform.GetComponent<EnemyController>();
            enemy.Dead();
            Dead();
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}