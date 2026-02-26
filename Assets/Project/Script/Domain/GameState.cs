using System.Collections.Generic;

namespace MemoryMatch.Domain
{
    [System.Serializable]
    public class GameState
    {
        public List<CardModel> Cards;
        public int Score;
        public int Rows;
        public int Columns;
    }
}