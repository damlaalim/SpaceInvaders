using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isLead;

    public bool IsAlive { get; set; } = true;

    public SpriteRenderer MySpriteRenderer => mySpriteRenderer;
    [SerializeField] private SpriteRenderer mySpriteRenderer;
    [SerializeField] private EnemyBulletController enemyBulletPrefabs;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag($"Wall") && isLead)
        {
            EnemyManager.Instance.ChangeDirection();
        }
    }

    public void Dead()
    {
        GameManager.Instance.DeadEnemy();
        
        transform.gameObject.SetActive(false);
        IsAlive = false;
        
        EnemyManager.Instance.SpawnDeadParticle(transform.position, mySpriteRenderer.color);
    }

    public void Shoot(float speed)
    {
        var bullet = Instantiate(enemyBulletPrefabs, transform.position, Quaternion.identity, GameManager.Instance.bulletParent.transform);
        bullet.MyStart(speed, mySpriteRenderer.color);
    }
}