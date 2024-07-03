using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SCN.Effect
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleObj : MonoBehaviour
	{
		public System.Action OnStop;

		public GameObject Sample;

		[SerializeField] bool able;
		ParticleSystem par;

		public bool Able
		{
			get => able;
			set => able = value;
		}
		public ParticleSystem Par
		{
			get
			{
				if (par == null)
				{
					par = GetComponent<ParticleSystem>();

					var main = Par.main;
					main.stopAction = ParticleSystemStopAction.Callback;
				}
				return par;
			}
		}

		void OnParticleSystemStopped()
		{
			OnStop?.Invoke();
		}
	}
}