using JetBrains.Annotations;
using UnityEngine;

namespace CrazySinger
{
    public class ButtonView : MonoBehaviour
    {
        [UsedImplicitly]
        public void PlayClickSound()
        {
            SoundController.I.Play(GameSound.Click, SoundChannel.UI);
        }
    }
}
