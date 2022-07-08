using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; set; }
    
    [SerializeField] public Vector2 gridSize;
    [SerializeField] private List<EnemyController> enemyPrefab = new List<EnemyController>();
    [SerializeField] private Vector2 offset;
    [SerializeField] private float speed;
    [SerializeField] private float direction = 1;
    [SerializeField] private float downValue;
    [SerializeField] private float shootSpeed;
    [SerializeField] private Vector2 shootDelayRange;
    [SerializeField] private List<Color> enemyColors = new List<Color>();
    [SerializeField] private ParticleSystem deadParticle;

    public List<EnemyController> _listEnemy = new List<EnemyController>();

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        CreateEnemies();
        
        StartShoot();
    }

    private void Update()
    {
        Move();
    }

    private void StartShoot()
    {
        IEnumerator Do()
        {
            yield return new WaitForSeconds(2);
            
            while (true)
            {
                var hasAlive = _listEnemy.Exists(x => x.IsAlive);
                if (!hasAlive) break;

                EnemyController enemyController;
                do
                {
                    enemyController = _listEnemy[Random.Range(0, _listEnemy.Count)];
                } while (!enemyController.IsAlive);
                
                enemyController.Shoot(shootSpeed);
                yield return new WaitForSeconds(Random.Range(shootDelayRange.x, shootDelayRange.y));
            }
        }

        StartCoroutine(Do());
    }
    
    public void ChangeDirection()
    {
        direction *= -1;

        var position = transform.position;
        position.y -= downValue;

        transform.position = position;
    }
    
    private void Move()
    {
        var position = transform.position;
        
        position.x += speed * Time.deltaTime * direction;

        transform.position = position;
    }
    
    public void SpawnDeadParticle(Vector3 position, Color color)
    {
        var particle = Instantiate(deadParticle, position, quaternion.identity);
        var main = particle.main;
        main.startColor = color;

        StartCoroutine(DestroyParticle());

        IEnumerator DestroyParticle()
        {
            yield return new WaitForSeconds(2);
            Destroy(particle.gameObject);
        }
    }
    
    [ContextMenu(nameof(CreateEnemies))]
    private void CreateEnemies()
    {
        transform.position = Vector3.zero;
        var position = Vector2.zero;
        
        for (int i = 0; i < gridSize.x; i++)
        {
            position.x = i * offset.x;
            
            for (int j = 0; j < gridSize.y; j++)
            {
                position.y = j * offset.y;

                var enemy = Instantiate(enemyPrefab[j], position, Quaternion.identity);
                _listEnemy.Add(enemy);
                
                enemy.transform.name = $"Enemy [{i},{j}]";
                enemy.MySpriteRenderer.color = enemyColors[j];

                enemy.transform.SetParent(transform);
                
                if (j == (gridSize.y - 1))
                    enemy.isLead = true;
            }
        }
        SetPositionCenter();
        transform.position = Vector2.up * 2;
    }

    private void SetPositionCenter()
    {
        var listChild = new List<Transform>();
        var position = Vector3.zero;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            listChild.Add(child);
            position += child.position;
        }

        position /= transform.childCount;

        foreach (var item in listChild)
            item.SetParent(transform.parent);

        transform.position = position;

        foreach (var item in listChild)
            item.SetParent(transform);            
    }
}