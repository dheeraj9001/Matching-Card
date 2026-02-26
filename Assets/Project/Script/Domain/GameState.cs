using MemoryMatch.Domain;
using System.Collections.Generic;

[System.Serializable]
public class GameState
{
    public int MatchCount;
    public int TurnCount;
    public int Rows;
    public int Columns;
    public List<CardModel> Cards;
}