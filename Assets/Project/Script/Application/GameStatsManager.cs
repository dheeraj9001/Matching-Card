using System;

namespace MemoryMatch.Domain
{
    public class GameStatsManager
    {
        public int MatchCount { get; private set; }
        public int TurnCount { get; private set; }

        public event Action<int, int> OnStatsChanged;
        public event Action<int> OnMatchCountChanged;

        public void RegisterTurn(bool isMatch)
        {
            TurnCount++;

            if (isMatch)
            {
                MatchCount++;
                OnMatchCountChanged?.Invoke(MatchCount); 
            }

            OnStatsChanged?.Invoke(MatchCount, TurnCount);
        }

        public void Reset()
        {
            MatchCount = 0;
            TurnCount = 0;
            OnStatsChanged?.Invoke(MatchCount, TurnCount);
        }

        public void ResetStats()
        {
            MatchCount = 0;
            TurnCount = 0;
            OnStatsChanged?.Invoke(MatchCount, TurnCount);
            OnMatchCountChanged?.Invoke(MatchCount);
        }
    }
}