using SCN.BinaryData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Image img;
    [SerializeField] List<Sprite> listSpr;

    public Button Btn { get => btn; set => btn = value; }

    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        btn.onClick.AddListener(()=> {
            if (img.sprite == listSpr[1])
            {
                img.sprite = listSpr[0];
                DataManagerSample.Instance.LocalData.isAudio = true;
                AudioManager.Ins.Play_Background_Music();
            }
            else
            {
                img.sprite = listSpr[1];
                AudioManager.Ins.Stop_Music();
                DataManagerSample.Instance.LocalData.isAudio = false;
            }
        });
    }
}
