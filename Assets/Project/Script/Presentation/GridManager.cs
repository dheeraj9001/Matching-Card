using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MemoryMatch.Domain;

namespace MemoryMatch.Presentation
{
    public class GridManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform container;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private CardSpriteLibrary spriteLibrary;

        private readonly List<CardView> _spawnedCards = new();

        public void GenerateGrid(int rows, int cols)
        {
            ClearGrid();
            ConfigureGrid(rows, cols);

            var cards = CreateShuffledPairs(rows * cols);

            foreach (var model in cards)
            {
                var view = Instantiate(cardPrefab, container);
                view.Initialize(model);
                view.OnClicked += gameManager.OnCardClicked;
                _spawnedCards.Add(view);
            }
        }

        private void ConfigureGrid(int rows, int cols)
        {
            float width = container.rect.width / cols;
            float height = container.rect.height / rows;
            float size = Mathf.Min(width, height);

            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = cols;
            grid.cellSize = new Vector2(size, size);
        }

        private List<CardModel> CreateShuffledPairs(int totalCards)
        {
            var list = new List<CardModel>();
            int pairCount = totalCards / 2;

            var sprites = spriteLibrary.GetShuffledSprites(pairCount);

            for (int i = 0; i < pairCount; i++)
            {
                var sprite = sprites[i];

                list.Add(new CardModel(i, sprite));
                list.Add(new CardModel(i, sprite));
            }

            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }

            return list;
        }

        private void ClearGrid()
        {
            foreach (var c in _spawnedCards)
                Destroy(c.gameObject);

            _spawnedCards.Clear();
        }

        public List<CardModel> GetAllCardModels()
        {
            var list = new List<CardModel>();

            foreach (var card in _spawnedCards)
            {
                list.Add(card.GetModel());
            }

            return list;
        }

        public void RefreshAllCards()
        {
            foreach (var card in _spawnedCards)
            {
                card.Refresh();
            }
        }
    }
}