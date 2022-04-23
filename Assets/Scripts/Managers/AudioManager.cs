using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioMain;
    public AudioClip MainMenuMusic;
    public AudioClip GameplayMusic;

    Animator musicFadeAnimator;

    void Awake()
    {
        audioMain = gameObject.GetComponent<AudioSource>();
        musicFadeAnimator = gameObject.GetComponent<Animator>();

        GameManager.OnGameStateChange += gameStateMusic;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChange -= gameStateMusic;
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
}
