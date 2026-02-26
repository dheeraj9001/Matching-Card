using UnityEngine;

namespace MemoryMatch.Infrastructure
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip flip;
        [SerializeField] private AudioClip match;
        [SerializeField] private AudioClip mismatch;
        [SerializeField] private AudioClip gameOver;
        [SerializeField] private AudioClip btnClick;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void PlayFlip() => sfxSource.PlayOneShot(flip);
        public void PlayMatch() => sfxSource.PlayOneShot(match);
        public void PlayMismatch() => sfxSource.PlayOneShot(mismatch);
        public void PlayGameOver() => sfxSource.PlayOneShot(gameOver);
        public void PlayButtonTap() => sfxSource.PlayOneShot(btnClick);
    }
}