using UnityEngine;
using Random = UnityEngine.Random;

namespace Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySound : MonoBehaviour
    {
        [Header("Random pool")] [SerializeField]
        private bool _useRandomPool;

        [SerializeField] private AudioClip[] _randomPool;

        [Header("Random pitch")] [SerializeField]
        private bool _useRandomPitch = true;

        [SerializeField] private Vector2 _randomPitch = new Vector2(-0.3f, 0.3f);
        [Header("General")] [SerializeField] private bool _playOnEnable;
        [SerializeField] private AudioSource _audioSource;

        public void Play()
        {
            if (_useRandomPitch)
            {
                _audioSource.pitch = 1f + Random.Range(_randomPitch.x, _randomPitch.y);
            }

            if (_useRandomPool)
            {
                var randomIndex = Random.Range(0, _randomPool.Length);
                _audioSource.clip = _randomPool[randomIndex];
            }

            _audioSource.Play();
        }

        private void Start()
        {
            if (_audioSource == null)
            {
                Reset();
            }
        }

        private void OnEnable()
        {
            if (_playOnEnable)
            {
                Play();
            }
        }

        private void Reset()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}