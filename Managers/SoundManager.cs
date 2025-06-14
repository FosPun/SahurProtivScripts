using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource coinsPickUpSound;
    GameManager gameManager;

    private int i = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinsPickUpSound = GetComponent<AudioSource>();
        gameManager = FindFirstObjectByType<GameManager>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (gameManager.coinsPicked > i)
        {
            coinsPickUpSound.Play();
            i++;
        }
            
    }
}
