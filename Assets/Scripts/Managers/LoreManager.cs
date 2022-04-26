using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    public GameObject LoreCanvas;
    public GameObject Lore1;
    public GameObject Lore2;
    public GameObject Lore3;

    Coroutine coroutine1, coroutine2, coroutine3;

    public void SkipPressed()
    {
        LoreCanvas.SetActive(false);
        GameManager.Instance.SwitchState(GameManager.Instance.PlayingState);

        if (coroutine1 != null) StopCoroutine(coroutine1);
        if (coroutine2 != null) StopCoroutine(coroutine2);
        if (coroutine3 != null) StopCoroutine(coroutine3);
    }

    public void StartTheLore()
    {
        showLore1();
        coroutine1 = StartCoroutine(WaitAndShowLore2());
    }

    IEnumerator WaitAndShowLore2()
    {
        yield return new WaitForSeconds(4);
        showLore2();
        coroutine2 = StartCoroutine(WaitAndShowLore3());
    }

    IEnumerator WaitAndShowLore3()
    {
        yield return new WaitForSeconds(4);
        showLore3();
        coroutine3 = StartCoroutine(WaitThenChangeState());
    }

    IEnumerator WaitThenChangeState()
    {
        yield return new WaitForSeconds(4);
        GameManager.Instance.SwitchState(GameManager.Instance.PlayingState);
    }

    void showLore1()
    {
        Lore1.SetActive(true);
        Lore2.SetActive(false);
        Lore3.SetActive(false);
    }

    void showLore2()
    {
        Lore1.SetActive(false);
        Lore2.SetActive(true);
        Lore3.SetActive(false);
    }

    void showLore3()
    {
        Lore1.SetActive(false);
        Lore2.SetActive(false);
        Lore3.SetActive(true);
    }
}
