using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SCN.BinaryData;

public class ItemShop : MonoBehaviour
{
    [SerializeField] ColorSO colorSO;
    [SerializeField] Image imgIcon;
    [SerializeField] Text price;
    [SerializeField] BtnSelectCar btnSelectCar;
    [SerializeField] BtnBuy btnBuy;
    [SerializeField] BtnActive btnActive;

    public ColorSO ColorSO { get => colorSO; set => colorSO = value; }

    private void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        imgIcon.sprite = ColorSO.Icon;
        imgIcon.SetNativeSize();
        price.text = ColorSO.Price.ToString();
        InitButton();
    }
    public bool HasIDData()
    {
        var id = DataManagerSample.Instance.LocalData.HasId(colorSO.Id);
        return id;
    }
    public bool HasIDChooseData()
    {
        if (DataManagerSample.Instance.LocalData.carChoose == colorSO.Id)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SwitchButton(BtnState state)
    {
        btnSelectCar.DeActive();
        btnBuy.DeActive();
        btnActive.DeActive();
        //
        if (state == BtnState.Select)
        {
            btnSelectCar.Active();
        }
        else if (state == BtnState.Buy)
        {
            btnBuy.Active();
        }
        else if (state == BtnState.Active)
        {
            btnActive.Active();
        }
    }

    void InitButton()
    {
        btnSelectCar.Button.onClick.AddListener(()=> {
            SwitchButton(BtnState.Active);
            DataManagerSample.Instance.LocalData.carChoose = colorSO.Id;
            ShopUI.OnSelect.Invoke();
            GameManager.Ins.DeActiveListCar();
            GameManager.Ins.ActiveCarChoose();
        });
        btnBuy.Button.onClick.AddListener(() => {
            var data = DataManagerSample.Instance.LocalData;
            if (data.pointDiamond >= colorSO.Price)
            {
                data.AddCar(colorSO.Id);
                data.BuyItemShop(colorSO.Price);
                SwitchButton(BtnState.Select);
                ShopUI.OnUpdatePoint.Invoke();
            }
            else
            {

            }
        });
    }
}