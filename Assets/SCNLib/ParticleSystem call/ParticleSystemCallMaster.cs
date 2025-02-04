using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCN.Effect
{
    public class ParticleSystemCallMaster : MonoBehaviour
    {
        static ParticleSystemCallMaster instance;
        public static ParticleSystemCallMaster Instance
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

        static void Setup()
        {
            var particleObj = new GameObject("Particle trans");
            particleObj.transform.SetParent(DDOL.Instance.transform);

            instance = particleObj.AddComponent<ParticleSystemCallMaster>();

            LoadSceneManager.BeforeExitScene += () =>
            {
				for (int i = 0; i < instance.parHolders.Count; i++)
				{
					if (!instance.parHolders[i].KeepOnLoadScene)
					{
                        instance.parHolders[i].DestroyPool();
                    }
                }
                instance.parHolders.RemoveAll(holder => !holder.KeepOnLoadScene);
            };
        }

        [SerializeField] List<ParticleHolder> parHolders = new List<ParticleHolder>();
        
        /// <summary>
        /// parPrefab truyền vào là Sample, hàm này trả về 1 "ParticleObj" là clone của 
        /// Sample truyền vào
        /// </summary>
        /// <param name="parent">Parent cua effect duoc spawn ra</param>
        public ParticleObj PlayParticle(Transform parent, GameObject prefab
            , bool keepHolderOnLoadScene = false)
        {
            var par = parHolders.Find(ph => ph.Prefab == prefab);

            if (par == null)
            {
                var newHolder = new ParticleHolder(prefab, transform, keepHolderOnLoadScene);
                parHolders.Add(newHolder);

                return PlayParticle(parent, prefab);
            }
			else
			{
                return par.Play(parent);
            }
        }

        public void StopParticle(ParticleObj parObj)
        {
            var holder = parHolders.Find(ph => ph.Prefab == parObj.Sample);
            if (holder != null) 
            {
                holder.Stop(parObj);
            }
        }

        [System.Serializable]
        public class ParticleHolder
        {
            [SerializeField] GameObject sample;
            [SerializeField] ObjectPool pool;
            [SerializeField] bool keepOnLoadScene;

            public ParticleHolder(GameObject sample, Transform trans, bool keepOnLoadScene)
			{
                this.sample = sample;
				pool = new ObjectPool(this.sample, trans);
                this.keepOnLoadScene = keepOnLoadScene;
            }

            public GameObject Prefab => sample;
            public bool KeepOnLoadScene => keepOnLoadScene;

            public ParticleObj Play(Transform parent)
            {
                var obj = pool.GetObjInPool();
                var parObj = obj.GetComponent<ParticleObj>();

                if (parObj == null)
				{
                    parObj = obj.AddComponent<ParticleObj>();
                    parObj.Sample = sample;
				}

                parObj.Able = true;

                parObj.gameObject.SetActive(true);
                parObj.transform.SetParent(parent);

                parObj.transform.localPosition = Vector2.zero;
                parObj.transform.localScale = Vector2.one;

                _ = DDOL.Instance.StartCoroutine(DelayCallMaster.WaitForEndOfFrame(() =>
                {
                    parObj.Par.Play();
                    parObj.OnStop = () =>
                    {
                        Remove(parObj);
                    };
                }));

                return parObj;
            }

            /// <summary>
            /// Stop particle, 
            /// </summary>
            public void Stop(ParticleObj parObj)
            {
                if (parObj.gameObject == null || !parObj.Able) return;

                parObj.Par.Stop();
            }

            public void DestroyPool()
			{
                pool.DestroyListAvailable();
			}

            /// <summary>
            /// Dua object ve Pool
            /// </summary>
            void Remove(ParticleObj parObj)
            {
                if (parObj == null || !parObj.Able) return;

                parObj.Able = false;
                parObj.gameObject.SetActive(false);
                pool.RemoveObj(parObj.gameObject);
            }
        }
    }
}