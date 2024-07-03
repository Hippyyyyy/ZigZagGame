using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnActive : BtnBase
{
    [SerializeField] Button button;

    public Button Button { get => button; set => button = value; }

    public override void Active()
    {
        gameObject.SetActive(true);
    }

    public override void DeActive()
    {
        gameObject.SetActive(false);
    }
}
