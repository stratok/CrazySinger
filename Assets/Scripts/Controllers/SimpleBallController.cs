using System;
using UnityEngine;

namespace CrazySinger
{
	public class SimpleBallController : MonoBehaviour
	{
		private int m_BallSens;
		private Rigidbody2D m_BallRigidbody;

		private void Awake()
		{
			m_BallRigidbody = GetComponent<Rigidbody2D>();
			m_BallSens = SaveController.LoadIntFromPrefs(Constants.Sensitivity, Constants.DefaultSensitivity);
		}

		private void Update()
		{
			m_BallRigidbody.AddForce(Vector3.up * InputController.MicValue * m_BallSens);
		}
		
		public void ChangeSensitivity(int value) => m_BallSens = value;
	}
}