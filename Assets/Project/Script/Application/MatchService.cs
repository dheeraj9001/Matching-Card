using System;

namespace MemoryMatch.Domain
{
    public class MatchService
    {
        private CardModel _first;
        private CardModel _second;

        public event Action<CardModel, CardModel, bool> OnPairEvaluated;

        public void SelectCard(CardModel card)
        {
            if (card.IsFaceUp || card.IsMatched)
                return;

            card.IsFaceUp = true;

            if (_first == null)
            {
                _first = card;
                return;
            }

            if (_second == null)
            {
                _second = card;
                Evaluate();
            }
        }

        private void Evaluate()
        {
            bool isMatch = _first.PairId == _second.PairId;

            if (isMatch)
            {
                _first.IsMatched = true;
                _second.IsMatched = true;
            }

            OnPairEvaluated?.Invoke(_first, _second, isMatch);

            _first = null;
            _second = null;
        }
    }
}