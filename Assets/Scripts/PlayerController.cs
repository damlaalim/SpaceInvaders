using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float horizontalLimit = 2.45f;
    [SerializeField] private BulletController bulletPrefab;

    private bool IsPressShootKey => Input.GetKeyDown(KeyCode.Space);
    
    private void Update()
    {
        Move();
        
        if (IsPressShootKey)
            Shoot();
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, GameManager.Instance.bulletParent.transform);
        bullet.MyStart(bulletSpeed);
    }
    
    private void Move()
    {
        var position = transform.position;
        
        // Set new position
        var input = Input.GetAxis("Horizontal");
        position.x += input * Time.deltaTime * speed;
        position.x = Mathf.Clamp(position.x, -horizontalLimit, horizontalLimit);
        
        transform.position = position;
    }

    public void Dead()
    {
        Debug.Log("Game Over");
        
        GameManager.Instance.EndGame();
    }
}