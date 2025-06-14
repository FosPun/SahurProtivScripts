using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    EnemyBehaviour enemyBehaviour;
    ScoreCount scoreCount;
    [SerializeField] private GameObject player;
    
   
    public int coinsPicked;
    [SerializeField] private int coinsToWin;
    

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text levelText;
    
    [SerializeField] public float speedLevelUpgrade = 0.5f;
    private AudioSource winSound;
    [SerializeField] private GameObject panelWinMenu;
    [SerializeField] private GameObject panelLoseMenu;
    private CanvasGroup cgWinMenu;
    private CanvasGroup cgLoseMenu;
    private AudioListener playerListener;
    private void Awake()
    {   
        player = GameObject.FindGameObjectWithTag("Player");
        playerListener = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        scoreCount = FindFirstObjectByType<ScoreCount>();
        winSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        WinCondition();
        LoseCondition();
        playerListener.enabled = !YG2.nowAdsShow;
        
    }

    private void LoseCondition()
    {
        if (YG2.saves.health != 0) return;
        YG2.SetLeaderboard("Score",YG2.saves.score);
        Destroy(scoreCount.gameObject);
        Destroy(this.gameObject);
        cgLoseMenu.alpha = 1;
        cgLoseMenu.interactable = true;
        cgLoseMenu.blocksRaycasts = true;
        Time.timeScale = 0f;
        YG2.saves.level = 1;
        YG2.saves.score = 0;
        YG2.saves.health = 3;
        YG2.SaveProgress();
    }
    
    public void DamagePlayer()
    {
        YG2.saves.health--;
        healthText.text = YG2.saves.health.ToString();
        // Телепортируем игрока
        player.transform.position = new Vector3(0, 1.5f, -5.5f);

        // Найдём всех призраков и отправим их на старт
        EnemyBehaviour[] enemies = FindObjectsByType<EnemyBehaviour>(FindObjectsSortMode.None);
        foreach (EnemyBehaviour enemy in enemies)
        {
            enemy.GoToStartPosition(); // вызов метода на каждом
        }
    }
    
    private void WinCondition()
    {
        if (coinsPicked != coinsToWin) return;
        Time.timeScale = 0f;
        cgWinMenu.alpha = 1;
        cgWinMenu.interactable = true;
        cgWinMenu.blocksRaycasts = true;
        coinsPicked = 0;
        winSound.Play();
        YG2.saves.health = 3;
        YG2.saves.level++;
        YG2.SaveProgress();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return; // если 0 — это главное меню
        panelWinMenu = GameObject.FindGameObjectWithTag("PanelWinMenu");
        panelLoseMenu = GameObject.FindGameObjectWithTag("PanelLoseMenu");
        cgWinMenu = panelWinMenu.GetComponent<CanvasGroup>();
        cgLoseMenu = panelLoseMenu.GetComponent<CanvasGroup>();
        levelText = GameObject.FindWithTag("LevelText")?.GetComponent<TMP_Text>();
        if (levelText != null) 
            levelText.text = YG2.saves.level.ToString();
        player = GameObject.FindGameObjectWithTag("Player");
        healthText = GameObject.FindWithTag("HealthText")?.GetComponent<TMP_Text>();
        if (healthText != null)
            healthText.text = YG2.saves.health.ToString();
    }
    
}
