using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioMain;
    public AudioClip MainMenuMusic;
    public AudioClip GameplayMusic;

    public AudioClip HappinessSound;
    public AudioClip CoinSound;

    Animator musicFadeAnimator;

    void Awake()
    {
        audioMain = gameObject.GetComponent<AudioSource>();
        musicFadeAnimator = gameObject.GetComponent<Animator>();

        GameManager.OnGameStateChange += gameStateMusic;
        GameManager.OnIncreasingPeople += OnIncreasingPeopleSound;
        GameManager.OnObtainCoins += OnObtainCoinsSound;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChange -= gameStateMusic;
        GameManager.OnIncreasingPeople -= OnIncreasingPeopleSound;
        GameManager.OnObtainCoins -= OnObtainCoinsSound;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioMain.clip = MainMenuMusic;
        GetComponent<AudioSource>().Play();
    }

    void gameStateMusic(BaseGameState gameState)
    {
        if (gameState == GameManager.Instance.MainMenuState)
        {
            audioMain.clip = MainMenuMusic;
            audioMain.Play();
        }

        if (gameState == GameManager.Instance.PlayingState)
        {
            StartCoroutine(changeMusic(GameplayMusic));
        }
    }

    IEnumerator changeMusic(AudioClip music)
    {
        musicFadeAnimator.SetTrigger("fadeOut");
        yield return new WaitForSeconds(2);
        musicFadeAnimator.SetTrigger("fadeIn");
        audioMain.clip = music;
        audioMain.Play();
    }

    void OnIncreasingPeopleSound()
    {
        audioMain.PlayOneShot(HappinessSound);
    }

    void OnObtainCoinsSound ()
    {
        audioMain.PlayOneShot(CoinSound);
    }
}
