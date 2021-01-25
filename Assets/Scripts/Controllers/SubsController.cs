using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrazySinger
{
	public class SubsController : GameLoopController
	{
		private RectTransform m_SubsPanel;
		private TextMeshProUGUI m_SubsText;

		private const float PanelShift = -120f;
		private const float ShiftDuration = 0.2f;
		
		private float m_StartTime;
		private float m_CurrentTime;
		private int m_StepCount;
		private int m_CurrentStep;

		protected override bool IsTimeLoop => false;

		private void Awake()
		{
			m_SubsPanel = FindObjectOfType<SubsPanelView>().GetComponent<RectTransform>();
			m_SubsText = FindObjectOfType<SubsTextView>().GetComponent<TextMeshProUGUI>();
			m_SubsText.text = string.Empty;
		}

		public override void Setup(GameController gameController, SongData songData)
		{
			base.Setup(gameController, songData);
			m_StepCount = SongData.SongSettings.Length;
		}

		public override void Play()
		{
			m_CurrentStep = 0;
			m_StartTime = Time.time;
			m_SubsPanel.DOAnchorPosY(PanelShift, 0);
			base.Play();
		}

		public override void Replay()
		{
			m_CurrentStep = 0;
			m_StartTime = Time.time;
			m_SubsPanel.DOAnchorPosY(PanelShift, 0);
			base.Replay();
		}

		protected override void GameLoop()
		{
			m_CurrentTime = (float)Math.Round(Time.time - m_StartTime, 1);

			if (m_CurrentStep < m_StepCount && m_CurrentTime >= SongData.SongSettings[m_CurrentStep].time)
			{
				ShowNextString(m_CurrentStep);
				m_CurrentStep++;
			}
		}

		private void ShowNextString(int stepNumber)
		{
			m_SubsPanel.DOAnchorPosY(PanelShift, ShiftDuration).OnComplete(() =>
			{
				m_SubsText.text = SongData.SongSettings[stepNumber].text;
				m_SubsPanel.DOAnchorPosY(0, ShiftDuration);
			});
		}
	}
}