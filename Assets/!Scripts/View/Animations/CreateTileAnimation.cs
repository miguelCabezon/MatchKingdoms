using System.Collections;
using UnityEngine;
using MVC.Model;

namespace MVC.View
{
    public class CreateTileAnimation : IViewAnimation
    {
        private Vector2Int _position;
        private TileModel _tileData;

        public CreateTileAnimation(Vector2Int position, TileModel tileData)
        {
            _position = position;
            _tileData = tileData;
        }

        public Coroutine PlayAnimation(BoardView board)
        {
            return board.StartCoroutine(AnimationCoroutine(board));
        }

        private IEnumerator AnimationCoroutine(BoardView board)
        {
            yield return null;
        }
    }
}