using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; set; }

    [SerializeField] private GameObject player;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI score;

    [SerializeField] public GameObject bulletParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        button.gameObject.SetActive(true);
    }

    public void DeadEnemy()
    {
        Score++;
        score.text = Score.ToString();

        var manager = EnemyManager.Instance;
        if (Score == manager.gridSize.x * manager.gridSize.y)
            EndGame();
    }
    
    public void StartGame()
    {
        text.text = "TAP TO PLAY";
        button.gameObject.SetActive(false);

        player.SetActive(true);
        EnemyManager.Instance.StartGame();

        Time.timeScale = 1;
    }

    public void EndGame()
    {
        Time.timeScale = 0;

        SceneManager.LoadScene("SampleScene");
    }
}