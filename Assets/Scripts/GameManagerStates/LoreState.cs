using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoreState : BaseGameState
{
    public override void EnterState(GameManager gameManager)
    {
        gameManager.LoreManager.GetComponent<LoreManager>().StartTheLore();
    }

    public override void UpdateState(GameManager gameManager)
    {
        //
    }
}