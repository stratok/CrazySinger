using System;
using UnityEngine;

namespace CrazySinger
{
	public class BallController : GameLoopController
	{
		private bool m_IsGameRunning;
		private int m_BallSens;
		private Rigidbody2D m_BallRigidbody;

		private BallCollisionType m_BallCollisionType = BallCollisionType.Bottom;

		protected override bool IsTimeLoop => false;

		private void Awake()
		{
			m_BallRigidbody = GetComponent<Rigidbody2D>();
			m_BallSens = SaveController.LoadIntFromPrefs(Constants.Sensitivity, Constants.DefaultSensitivity);
		}

		public void OnCollisionEnter2D(Collision2D collision)
		{
			if (!m_IsGameRunning)
				return;
			
			if (collision.gameObject.CompareTag(Constants.TagObstacles))
				Lose();
			else if (collision.gameObject.CompareTag(Constants.TagFinish))
				Win();
			else if (collision.gameObject.CompareTag(Constants.TagGroundTop))
				m_BallCollisionType = BallCollisionType.Top;
			else if (collision.gameObject.CompareTag(Constants.TagGroundBottom))
				m_BallCollisionType = BallCollisionType.Bottom;
		}
		
		public void OnCollisionExit2D(Collision2D collision) => m_BallCollisionType = BallCollisionType.None;

		private void Lose()
		{
			m_IsGameRunning = false;
			GameController.LoseGame();
		}

		private void Win()
		{
			m_IsGameRunning = false;
			GameController.WinGame();
		}

		private void Rotate()
		{
			if (m_BallCollisionType == BallCollisionType.Bottom)
				transform.Rotate(Vector3.forward, -SongData.BallSpeed * Time.deltaTime);
			else if (m_BallCollisionType == BallCollisionType.Top)
				transform.Rotate(Vector3.forward, SongData.BallSpeed * Time.deltaTime);
		}
		
		private void Move()
		{
			m_BallRigidbody.AddForce(Vector3.up * InputController.MicValue * m_BallSens);
		}
		
		public void ChangeSensitivity(int value) => m_BallSens = value;
		
		public void ResetCollisions() => m_BallCollisionType = BallCollisionType.None;

		protected override void GameLoop()
		{
			Move();
			Rotate();
		}

		public override void Play()
		{
			base.Play();
			m_IsGameRunning = true;
			m_BallCollisionType = BallCollisionType.Bottom;
		}
	}
}