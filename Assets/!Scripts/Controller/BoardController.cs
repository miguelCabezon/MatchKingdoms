using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private BoardModel model = new BoardModel();
    private bool _isShuffling = false;

    // private void UpdateMovementUI()
	// {
	// 	_movementCounterText.SetText($"{_movesRemaining}");

	// 	if (_movesRemaining == 0 && ScoreCounter.Instance.Score < _scoreObjective && _actualState == State.GAME)
	// 	{
	// 		OnLoseAction?.Invoke();
	// 	}
	// }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shuffle() // Shuffle all tiles
	{
		_isShuffling = true;

		foreach (var row in model._rows)
			foreach (var tile in row.Tiles)
				tile.Type = model._tileTypes[Random.Range(0, model._tileTypes.Length)];

		_isShuffling = false;
	}
}
