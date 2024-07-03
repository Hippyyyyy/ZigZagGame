using SCN.BinaryData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameUI : UIBase
{
    [SerializeField] UIState state;
    [SerializeField] Text mainTxt;
    [SerializeField] Button btnAds;
    [SerializeField] Button btnAudio;
    public override void EnterState()
    {
        gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        InitButton();
    }

    void InitButton()
    {
        btnAds.onClick.AddListener(()=> {
            var data = DataManagerSample.Instance.LocalData;
            AdsManager.Instance.ShowRewardVideo(()=> {
                data.AddPointDiamondAds();
            });
        });
    }
}
