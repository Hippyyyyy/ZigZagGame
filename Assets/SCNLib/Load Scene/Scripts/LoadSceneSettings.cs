using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SCN.Common
{
    [CreateAssetMenu(fileName = AssetName, menuName = "SCN/Scriptable Objects/LoadScene")]
    public class LoadSceneSettings : ScriptableObject
    {
        public const string AssetName = "Load scene settings";

        [Header("Custom")]
        [SerializeField] string layer;
        [SerializeField] int orderInLayer;

        [SerializeField] AnimLoadSceneBase loadSceneAnimDefault;

        public string Layer => layer;
        public int OrderInLayer => orderInLayer;
        public AnimLoadSceneBase LoadSceneAnimDefault => loadSceneAnimDefault;

        public void Preload()
		{
            LoadSceneManager.Instance.Preload();
		}

#if UNITY_EDITOR
        [MenuItem("SCN/Load scene/Settings")]
        static void SelectSettingLoadSceneConfig()
        {
            Master.CreateAndSelectAssetInResource<LoadSceneSettings>(AssetName);
        }
#endif
    }
}