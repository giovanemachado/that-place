using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerErrorManager : MonoBehaviour
{
    public GameObject ErrorLog;
    public TextMeshProUGUI ErrorDescriptionText;

    void Awake()
    {
        GameManager.OnPlayerError += GameManagerOnPlayerError;
    }

    void OnDestroy()
    {
        GameManager.OnPlayerError -= GameManagerOnPlayerError;
    }

    void GameManagerOnPlayerError(string error)
    {
        ErrorDescriptionText.text = error;
        ErrorLog.SetActive(true);
        StartCoroutine(CloseErrorLog());
    }

    IEnumerator CloseErrorLog()
    {
        yield return new WaitForSeconds(3);
        ErrorLog.SetActive(false);
    }
}
