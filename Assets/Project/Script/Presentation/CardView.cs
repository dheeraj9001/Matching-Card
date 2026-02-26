using DG.Tweening;
using MemoryMatch.Domain;
using MemoryMatch.Infrastructure;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryMatch.Presentation
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image front;
        [SerializeField] private GameObject back;
        [SerializeField] private float flipDuration = 0.25f;

        private Tween flipTween;
        private CardModel _model;

        public event Action<CardView> OnClicked;

        public void Initialize(CardModel model)
        {
            _model = model;
            button.onClick.AddListener(HandleClick);
            Refresh();
        }

        private void HandleClick()
        {
            OnClicked?.Invoke(this);
        }

        public void Refresh()
        {
            if (front != null)
                front.sprite = _model.FrontSprite;

            PlayFlip(_model.IsFaceUp);
        }

        public CardModel GetModel() => _model;

        public void PlayFlip(bool faceUp)
        {
            flipTween?.Kill();

            flipTween = DOTween.Sequence()
                .Append(transform.DOScaleX(0f, flipDuration * 0.5f))
                .AppendCallback(() =>
                {
                    back.SetActive(!faceUp);
                    AudioManager.Instance?.PlayFlip();
                })
                .Append(transform.DOScaleX(1f, flipDuration * 0.5f));
        }
    }
}