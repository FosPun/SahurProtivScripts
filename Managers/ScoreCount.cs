using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class ScoreCount : MonoBehaviour
{
    private static ScoreCount Instance;
    [SerializeField] private TMP_Text scoreText;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // уничтожаем дубликат
        }
    }
    
    public void AddScore(int scoreAmount)
    {
        YG2.saves.score += scoreAmount;
        scoreText.text = YG2.saves.score.ToString();
        //Debug.Log(_score);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scoreText = GameObject.FindWithTag("ScoreText")?.GetComponent<TMP_Text>();
        if (scoreText != null)
            scoreText.text = YG2.saves.score.ToString();
    }
}