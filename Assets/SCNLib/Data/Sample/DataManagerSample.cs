using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCN.BinaryData
{
    /// <summary>
    /// Script mau de quan ly data
    /// </summary>
    [CreateAssetMenu(fileName = fileNameSO, menuName = "SCN sample/Scriptable Objects/Data")]
    public class DataManagerSample : ScriptableObject
    {
        const string fileNameSO = "Data manager";

        static DataManagerSample instance;
        public static DataManagerSample Instance
        {
            get
            {
                if (instance == null) Setup();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        [SerializeField] string dataFileName = "data.scn";

        static void Setup()
        {
            // Load SO Data
            instance = LoadSource.LoadObject<DataManagerSample>(fileNameSO);

            instance.localData = BinaryDataManager.LoadData<LocalData>(instance.dataFileName);

            // Auto save khi quit game
            DDOL.Instance.OnApplicationPauseE += pause => { Debug.Log("local data " + pause); if (pause) instance.SaveLocalData(); };
            DDOL.Instance.OnApplicationQuitE += () => { instance.SaveLocalData(); };

#if UNITY_EDITOR
            DDOL.Instance.OnUpdateE += () => { if (Input.GetKeyDown(KeyCode.S)) instance.SaveLocalData(); };
#endif
        }

        [SerializeField] LocalData localData;
        public LocalData LocalData => localData;

        public void SaveLocalData()
        {
            Debug.Log("Save data");
            BinaryDataManager.SaveData<LocalData>(localData, dataFileName);
        }
    }

    /// <summary>
    /// Cau truc mau cua 1 class data
    /// </summary>
    [System.Serializable]
    public class LocalData : BinaryData
    {
        public bool IsReplay = false;
        public List<int> listCarInt = new List<int>();
        public int carChoose;
        public int pointDiamond;
        public bool isAudio = true;
        public override void SetupDefault()
        {
            // Trang thai dau tien cua data, mac dinh khi bat dau
            IsReplay = false;
            listCarInt = new List<int>();
            listCarInt.Add(0);
            carChoose = 0;
            pointDiamond = 1000;
        }
        public void AddCar(int id)
        {
            if (!listCarInt.Contains(id))
            {
                listCarInt.Add(id);
            }
        }
        public void RemoveCar(int id)
        {
            if (listCarInt.Contains(id))
            {
                listCarInt.Remove(id);
            }
        }
        public bool HasId(int id)
        {
            return listCarInt.Contains(id);
        }
        public int GetCar(int id)
        {
            return listCarInt[id];
        }
        public void SetCarChoose(int carChooseid)
        {
            carChoose = carChooseid;
        }
        public void AddPointDiamond()
        {
            var temp = pointDiamond;
            pointDiamond = temp + 1;
        }
        public void AddPointDiamondAds()
        {
            var temp = pointDiamond;
            pointDiamond = temp + 100;
        }
        public void BuyItemShop(int id)
        {
            var temp = pointDiamond;
            pointDiamond = temp - id;
        }
    }
}