using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SCN.BinaryData;
using UnityEngine.UI;
using System;

public class ShopUI : UIBase
{
    [SerializeField] UIState state;
    [SerializeField] Transform content;
    [SerializeField] ItemShop itemShop;
    [SerializeField] List<ItemShop> listItemShop;
    [SerializeField] List<ColorSO> listColor;
    [SerializeField] Text gems;

    public static Action OnSelect;
    public static Action OnUpdatePoint;

    public List<ColorSO> ListColor { get => listColor; set => listColor = value; }

    private void OnEnable()
    {
        OnSelect += ActiveSelect;
        OnUpdatePoint += UpdatePointDiamond;
    }

    private void OnDisable()
    {
        OnSelect = null;
        OnUpdatePoint = null;
    }

    private void OnDestroy()
    {
        OnSelect = null;
        OnUpdatePoint = null;
    }

    public override void EnterState()
    {
        gameObject.SetActive(true);
        UpdatePointDiamond();
    }

    public override void ExitState()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < 10; i++)
        {
            var item = Instantiate(itemShop, content);
            item.ColorSO = ListColor[i];
            item.gameObject.SetActive(true);
            item.SwitchButton(BtnState.Buy);
            listItemShop.Add(item);
        }

        for (int i = 0; i < listItemShop.Count; i++)
        {
            var itemShop = listItemShop[i];
            if (itemShop.HasIDData())
            {
                if (itemShop.HasIDChooseData())
                {
                    itemShop.SwitchButton(BtnState.Active);
                }
                else
                {
                    itemShop.SwitchButton(BtnState.Select);
                }
            }
            else
            {
                itemShop.SwitchButton(BtnState.Buy);
            }
        }
        
    }

    public void ActiveSelect()
    {
        for (int i = 0; i < listItemShop.Count; i++)
        {
            var itemShop = listItemShop[i];
            if (itemShop.HasIDData())
            {
                if (itemShop.HasIDChooseData())
                {
                    itemShop.SwitchButton(BtnState.Active);
                }
                else
                {
                    itemShop.SwitchButton(BtnState.Select);
                }
            }
            else
            {
                itemShop.SwitchButton(BtnState.Buy);
            }
        }
        GameManager.Ins.DeActiveListCar();
        GameManager.Ins.ActiveCarChoose();
    }

    public void UpdatePointDiamond()
    {
        var dataGems = DataManagerSample.Instance.LocalData.pointDiamond;
        gems.text = "Gems: " + dataGems;
    }
}
