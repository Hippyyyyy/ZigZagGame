using SCN.BinaryData;
using SCN.Effect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Ins;
    [SerializeField] bool gameStarted;
    [SerializeField] GameObject platformSpawner;
    [SerializeField] Material matColor;
    int score = 0;
    int highScore;
    [SerializeField] Text scoreTxt;
    [SerializeField] Text highScoreTxt;
    [SerializeField] Transform GamePlayUI;
    [SerializeField] Transform MenuGame;
    [SerializeField] Button tapStart;
    [SerializeField] Transform NameMenutxt;
    [SerializeField] Button btnShop;
    [SerializeField] Button btnExitShop;
    [SerializeField] ShopUI shopUI;
    [SerializeField] List<UIBase> uIBases;
    [SerializeField] List<CarController> listCar;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] GameObject particle;

    public bool GameStarted { get => gameStarted; set => gameStarted = value; }

    private void Awake()
    {
        Ins = this;
        var data = DataManagerSample.Instance.LocalData.IsReplay;
        if (!data)
        {
            SwitchState(UIState.Menu);
        }
        else
        {
            GameStart();
        }
        SpawnCar();
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreTxt.text = "High Score: " + highScore.ToString();
        InitButtonStart();
        InitButtonShop();
        InitButtonExitShop();
        AudioManager.Ins.Play_Background_Music();
    }

    void InitButtonStart()
    {
        tapStart.onClick.AddListener(() =>
        {
            if (!GameStarted)
            {
                GameStart();
            }
        });
    }

    private void Update()
    {
        /*if (!GameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
        }*/
    }

    public void GameStart()
    {
        GameStarted = true;
        DataManagerSample.Instance.LocalData.IsReplay = false;
        platformSpawner.SetActive(true);
        SwitchState(UIState.GamePlay);
        cameraFollow.ChangerBackground.StartChange();
    }
    public void GameOver()
    {
        GameStarted = false;
        SwitchState(UIState.GameOver);
        platformSpawner.SetActive(false);
        SaveHighScore();
    }

    void SaveHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            if (score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void IncrementScore()
    {
        score += 2;
        scoreTxt.text = score.ToString();
    }

    public void SwitchState(UIState state)
    {
        for (int i = 0; i < uIBases.Count; i++)
        {
            var ui = uIBases[i];
            ui.ExitState();
        }
        if (state == UIState.Menu)
        {
            uIBases[0].EnterState();
        }
        else if (state == UIState.GamePlay)
        {
            uIBases[1].EnterState();
        }
        else if (state == UIState.GameOver)
        {
            uIBases[2].EnterState();
        }
        else if (state == UIState.Shop)
        {
            uIBases[3].EnterState();
        }
    }

    public void InitButtonShop()
    {
        btnShop.onClick.AddListener(() =>
        {
            SwitchState(UIState.Shop);
        });
    }
    public void InitButtonExitShop()
    {
        btnExitShop.onClick.AddListener(() =>
        {
            SwitchState(UIState.Menu);
        });
    }

    public void SpawnCar()
    {
        var data = DataManagerSample.Instance.LocalData;
        for (int i = 0; i < 10; i++)
        {
            var car = Instantiate(shopUI.ListColor[i].Car, new Vector3(0,1.5f,0), Quaternion.identity);
            listCar.Add(car.GetComponent<CarController>());
        }
        DeActiveListCar();
        ActiveCarChoose();
    }

    public void DeActiveListCar()
    {
        for (int i = 0; i < 10; i++)
        {
            listCar[i].gameObject.SetActive(false);
        }
    }
    public void ActiveCarChoose()
    {
        var data = DataManagerSample.Instance.LocalData;
        listCar[data.carChoose].gameObject.SetActive(true);
        cameraFollow.Target = listCar[data.carChoose].transform;
    }

    public void Parrticle(Transform parent)
    {
        ParticleSystemCallMaster.Instance.PlayParticle(parent, particle);
    }
}
