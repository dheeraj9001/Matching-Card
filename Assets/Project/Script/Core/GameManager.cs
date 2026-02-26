using MemoryMatch.Application;
using MemoryMatch.Domain;
using MemoryMatch.Infrastructure;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMatch.Presentation
{
    public class GameManager : MonoBehaviour
    {
        private MatchService _matchService;
        private ScoreManager _scoreManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Text scoreText;
        [SerializeField] private int rows = 4;
        [SerializeField] private int cols = 4;

        private void Awake()
        {
            _matchService = new MatchService();
            _scoreManager = new ScoreManager();
            _matchService.OnPairEvaluated += HandlePairEvaluated;
            _scoreManager.OnScoreChanged += HandleScoreChanged;
        }

        private void Start()
        {
            var save = SaveSystem.Load();

            if (save != null)
            {
                rows = save.Rows;
                cols = save.Columns;
                _scoreManager = new ScoreManager();
                _scoreManager.Reset();
                _scoreManager.AddMatch(); 
                gridManager.GenerateGrid(rows, cols);
            }
            else
            {
                gridManager.GenerateGrid(rows, cols);
            }
        }

        public void OnCardClicked(CardView view)
        {
            _matchService.SelectCard(view.GetModel());
            view.Refresh();
        }

        private void HandlePairEvaluated(CardModel a, CardModel b, bool isMatch)
        {
            if (isMatch)
            {
                _scoreManager.AddMatch();
                AudioManager.Instance?.PlayMatch();
            }
            else
            {
                _scoreManager.AddMismatch();
                AudioManager.Instance?.PlayMismatch();
                StartCoroutine(FlipBackRoutine(a, b));
            }

            SaveProgress();
        }

        private void HandleScoreChanged(int score)
        {
            if (scoreText != null)
                scoreText.text = score.ToString();
        }

        private System.Collections.IEnumerator FlipBackRoutine(CardModel a, CardModel b)
        {
            yield return new WaitForSeconds(0.5f);

            a.IsFaceUp = false;
            b.IsFaceUp = false;

            gridManager.RefreshAllCards(); 
        }

        private void SaveProgress()
        {
            GameState state = new GameState
            {
                Score = _scoreManager.Score,
                Rows = rows,
                Columns = cols,
                Cards = gridManager.GetAllCardModels()
            };

            SaveSystem.Save(state);
        }
    }
}
