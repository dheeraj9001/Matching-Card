using MemoryMatch.Domain;
using MemoryMatch.Infrastructure;
using UnityEngine;
using UnityEngine.UI;


namespace MemoryMatch.Presentation
{
    public class GameManager : MonoBehaviour
    {
        private MatchService _matchService;
        private GameStatsManager _gameStatsManager;

        [SerializeField] private GameObject gameStartScreen;
        [SerializeField] private Button playBtn;
        [SerializeField] private GameObject gamePausePop;
        [SerializeField] private Button gamePauseBtn;
        [SerializeField] private Button gameResumeBtn;
        [SerializeField] private Button homeBtn;
        [SerializeField] private GameObject restartGameScreen;
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button restartHomeBtn;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Text matchText;
        [SerializeField] private Text turnText;
        [SerializeField] private int rows = 4;
        [SerializeField] private int cols = 4;

        private int _totalPairs;
        private bool _isGameComplete;

        private void Awake()
        {
            _matchService = new MatchService();
            _gameStatsManager = new GameStatsManager();
        }

        private void OnEnable()
        {
            _matchService.OnPairEvaluated += HandlePairEvaluated;
            _gameStatsManager.OnStatsChanged += HandleStatsChanged;
            _gameStatsManager.OnMatchCountChanged += CheckGameComplete;
            RegisterListeners();
        }

        private void OnDisable()
        {
            _matchService.OnPairEvaluated += HandlePairEvaluated;
            _gameStatsManager.OnStatsChanged -= HandleStatsChanged;
            _gameStatsManager.OnMatchCountChanged -= CheckGameComplete;
            UnregisterListeners();
        }

        private void Start()
        {
            ShowStartScreen();

            var save = SaveSystem.Load();

            if (save != null)
            {
                rows = save.Rows;
                cols = save.Columns;
            }

            _totalPairs = (rows * cols) / 2;
            _isGameComplete = false;

            gridManager.GenerateGrid(rows, cols);

        }

        private void RegisterListeners()
        {
            playBtn.onClick.AddListener(RestartGame);
            gamePauseBtn.onClick.AddListener(PauseGame);
            gameResumeBtn.onClick.AddListener(ResumeGame);
            homeBtn.onClick.AddListener(ShowStartScreen);
            restartBtn.onClick.AddListener(RestartGame);
            restartHomeBtn.onClick.AddListener(ShowStartScreen);
        }

        private void UnregisterListeners()
        {
            playBtn.onClick.RemoveListener(RestartGame);
            gamePauseBtn.onClick.RemoveListener(PauseGame);
            gameResumeBtn.onClick.RemoveListener(ResumeGame);
            homeBtn.onClick.RemoveListener(ShowStartScreen);
            restartBtn.onClick.RemoveListener(RestartGame);
            restartHomeBtn.onClick.RemoveListener(ShowStartScreen);
        }

        private void ShowStartScreen()
        {
            gameStartScreen.SetActive(true);
            gamePausePop.SetActive(false);
            restartGameScreen.SetActive(false);
            Time.timeScale = 1;
        }

        private void ShowGameplayUI()
        {
            AudioManager.Instance.PlayButtonTap();
            gameStartScreen.SetActive(false);
            gamePausePop.SetActive(false);
            restartGameScreen.SetActive(false);
            Time.timeScale = 1;
        }
        private void PauseGame()
        {
            AudioManager.Instance.PlayButtonTap();
            Time.timeScale = 0;
            gamePausePop.SetActive(true);
        }

        private void ResumeGame()
        {
            AudioManager.Instance.PlayButtonTap();
            Time.timeScale = 1;
            gamePausePop.SetActive(false);
        }


        private void RestartGame()
        {
            AudioManager.Instance.PlayButtonTap();

            _isGameComplete = false;
            _gameStatsManager.ResetStats();
            _matchService.ResetSelection();

            gridManager.ResetBoard();
            ShowGameplayUI();
        }

        public void OnCardClicked(CardView view)
        {
            _matchService.SelectCard(view.GetModel());
            view.Refresh();
        }

        private void HandlePairEvaluated(CardModel a, CardModel b, bool isMatch)
        {

            _gameStatsManager.RegisterTurn(isMatch);

            if (isMatch)
            {
                AudioManager.Instance?.PlayMatch();
            }
            else
            {
                AudioManager.Instance?.PlayMismatch();
                StartCoroutine(FlipBackRoutine(a, b));
            }

            SaveProgress();
        }

        private void CheckGameComplete(int currentMatches)
        {
            if (_isGameComplete) return;

            if (currentMatches >= _totalPairs)
            {
                _isGameComplete = true;
                OnGameCompleted();
            }
        }


        private void OnGameCompleted()
        {
            Debug.Log(" Game Completed!");
            restartGameScreen.SetActive(true);
        }


        private System.Collections.IEnumerator FlipBackRoutine(CardModel a, CardModel b)
        {
            yield return new WaitForSeconds(0.5f);

            a.IsFaceUp = false;
            b.IsFaceUp = false;

            gridManager.RefreshAllCards();
        }

        private void HandleStatsChanged(int matches, int turns)
        {
            if (matchText != null)
                matchText.text = $"Matches: {matches}";

            if (turnText != null)
                turnText.text = $"Turns: {turns}";
        }

        private void SaveProgress()
        {
            GameState state = new GameState
            {
                MatchCount = _gameStatsManager.MatchCount,
                TurnCount = _gameStatsManager.TurnCount,
                Rows = rows,
                Columns = cols,
                Cards = gridManager.GetAllCardModels()
            };
        }
    }
}
