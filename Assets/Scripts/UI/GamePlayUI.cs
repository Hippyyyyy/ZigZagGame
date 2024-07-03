using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : UIBase
{
    [SerializeField] UIState state;
    [SerializeField] Text scoreTxt;
    int score = 0;

    public override void EnterState()
    {
        gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        gameObject.SetActive(false);
    }
}
