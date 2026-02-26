using UnityEngine;

namespace MemoryMatch.Domain
{
    [System.Serializable]
    public class CardModel
    {
        public int PairId;
        public Sprite FrontSprite;
        public bool IsFaceUp;
        public bool IsMatched;

        public CardModel(int pairId, Sprite sprite)
        {
            PairId = pairId;
            FrontSprite = sprite;
            IsFaceUp = false;
            IsMatched = false;
        }
    }
}