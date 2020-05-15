using UnityEngine;

namespace Normal.Realtime.Native {
    public class AudioPreprocessorPlaybackListener : MonoBehaviour {
        public AudioPreprocessor audioPreprocessor;
        private bool didLogChannelWarning = false;

        void OnAudioFilterRead(float[] samples, int channels) {
            if (audioPreprocessor == null)
                return;

            if (channels != 2) {
                if (!didLogChannelWarning) {
                    Debug.LogWarning("AudioPreprocessorPlaybackListener asked to process a non-stereo signal. Echo cancellation will not work.");
                    didLogChannelWarning = true;
                }
                return;
            }

            audioPreprocessor.ProcessPlaybackFrame(samples);
        }
    }
}
