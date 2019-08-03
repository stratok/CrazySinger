using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CrazySinger
{
	public class SubsController : GameLoopController
	{
		private SongData _songData;
		private RectTransform _subsPanel;
		private Text _subsText;
		private RectTransform _finishView;

		private SoundController _soundCtr;

		private float _panelShift = -120f;
		private float _shiftDuration = 0.2f;
		private float _startTime;
		private float _currentTime;
		private int _stepCount;
		private int _currentStep;

		protected override bool IsTimeLoop => false;

		protected override void Setup()
		{
			_songData = FindObjectOfType<SongDataContainer>().SongData;
			_subsPanel = FindObjectOfType<SubsPanelView>().GetComponent<RectTransform>();
			_subsText = FindObjectOfType<SubsTextView>().GetComponent<Text>();
			_soundCtr = FindObjectOfType<SoundController>();
			_finishView = FindObjectOfType<FinalView>().GetComponent<RectTransform>();

			_stepCount = _songData.SongSettings.Length;
			_subsText.text = string.Empty;
		}

		public override void Play()
		{
			base.Play();

			_currentStep = 0;
			_startTime = Time.time;
		}

		public override void Replay()
		{
			_currentStep = 0;
			_subsPanel.DOAnchorPosY(_panelShift, 0);
			base.Replay();
		}

		protected override void GameLoop()
		{
			_currentTime = (float)Math.Round(Time.time - _startTime, 1);

			if (_currentStep < _stepCount && _currentTime == _songData.SongSettings[_currentStep].time)
			{
				_currentStep++;
				ShowNextString();
			}
			else if (_currentTime == _songData.FinalTime)
			{
				StartCoroutine(ShowFinish());
			}
		}

		private IEnumerator ShowFinish()
		{
			_subsPanel.DOAnchorPosY(_panelShift, _shiftDuration);
			_soundCtr.Play(SoundsList.Finish);
			_finishView.DOAnchorPosX(0, 0.3f);
			yield return new WaitForSeconds(2);
			_finishView.DOAnchorPosX(400, 0.3f);
		}

		private void ShowNextString()
		{
			_subsPanel.DOAnchorPosY(_panelShift, _shiftDuration).OnComplete(() =>
			{
				_subsText.text = _songData.SongSettings[_currentStep - 1].text;
				_subsPanel.DOAnchorPosY(0, _shiftDuration);
			});
		}
	}
}