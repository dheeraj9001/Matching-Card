using System;
using UnityEngine;

namespace MemoryMatch.Application
{
    public class ScoreManager
    {
        public int Score { get; private set; }

        public event Action<int> OnScoreChanged;

        public void AddMatch()
        {
            Score += 100;
            OnScoreChanged?.Invoke(Score);
        }

        public void AddMismatch()
        {
            Score = Mathf.Max(0, Score - 10);
            OnScoreChanged?.Invoke(Score);
        }

        public void Reset()
        {
            Score = 0;
            OnScoreChanged?.Invoke(Score);
        }
    }
}