using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject LeaderBoard;
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowRecords()
    {
        LeaderBoard.SetActive(true);
    }
}
