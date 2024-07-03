using DG.Tweening;
using SCN.BinaryData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : UIBase
{
    [SerializeField] Button replay;
    [SerializeField] Button mainMenu;
    [SerializeField] Text mainTxt;

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
        Replay();
        MainMenu();
    }

    public void Replay()
    {
        replay.onClick.AddListener(() =>
        {
            AdsManager.Instance.ShowInterstitial(() =>
            {
                DataManagerSample.Instance.LocalData.IsReplay = true;
                replay.onClick.RemoveAllListeners();
                replay.transform.DOScale(1.1f, 0.3f).OnComplete(() =>
                {
                    replay.transform.DOScale(1f, 0.1f).OnComplete(() =>
                    {
                        SceneManager.LoadScene("Game");
                    });
                });
            });
        });
    }
    public void MainMenu()
    {
        mainMenu.onClick.AddListener(() =>
        {
            AdsManager.Instance.ShowInterstitial(() =>
            {
                DataManagerSample.Instance.LocalData.IsReplay = false;
                mainMenu.onClick.RemoveAllListeners();
                mainMenu.transform.DOScale(1.1f, 0.3f).OnComplete(() =>
                {
                    mainMenu.transform.DOScale(1f, 0.1f).OnComplete(() =>
                    {
                        SceneManager.LoadScene("Game");
                    });
                });
            });
        });
    }
}
