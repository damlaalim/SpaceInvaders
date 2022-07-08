using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidRigidbody2D;
    [SerializeField] private SpriteRenderer mySpriteRenderer;

    public void MyStart(float speed, Color color)
    {
        // TODO: bulletlar türesin  
        IEnumerator Do()
        {
            mySpriteRenderer.color = color;
            myRigidRigidbody2D.AddForce(Vector2.down * speed);
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        StartCoroutine(Do());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag($"Player"))
        {
            var player = col.transform.GetComponent<PlayerController>();
            player.Dead();
            Dead();
        }
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}