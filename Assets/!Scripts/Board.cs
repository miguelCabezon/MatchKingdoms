using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using TMPro;
using System.Runtime.CompilerServices;

public sealed class Board : MonoBehaviour
{
	#region Victory Conditions
	[SerializeField] private int _scoreObjective = 200;
	[SerializeField] private int _movesLimit = 10;
	private int _movesRemaining = 0;
	[SerializeField] private TMP_Text _movementCounterText; // * This should not be removed

	[SerializeField] private GameObject _gameOverScreen;
	[SerializeField] private GameObject _winScreen;

	// public enum State
	// {
	// 	WIN,
	// 	LOSE,
	// 	NONE
	// }

	// private State _actualState = State.NONE;

	
	#endregion


	[SerializeField] private TileType[] _tileTypes;
	[SerializeField] private Row[] _rows;

	#region Audio
	[SerializeField] private AudioClip _matchSound;
	[SerializeField] private AudioClip _endMatchSound;
	[SerializeField] private AudioSource audioSource;
	#endregion

	[SerializeField] private float _tweenDuration;
	[SerializeField] private Transform _swappingOverlay;
	[SerializeField] private bool _ensureNoStartingMatches;

	//TODO: This selection now works with two clicks but should work with drag.
	private readonly List<Tile> _selection = new List<Tile>();

	#region Inner States
	private bool _isSwapping;
	private bool _isMatching;
	private bool _isShuffling;
	#endregion

	public event Action<TileType, int> OnMatchAction;

	private TileData[,] Matrix // Matrix filled with tile info
	{
		get
		{
			var width = _rows.Max(row => row.Tiles.Length);
			var height = _rows.Length;

			var data = new TileData[width, height];

			for (var y = 0; y < height; y++)
				for (var x = 0; x < width; x++)
					data[x, y] = GetTile(x, y).Data;

			return data;
		}
	}

	private void Awake()
	{
		_movesRemaining = _movesLimit;
		UpdateMovementUI();
	}
		

	private void Start() => BuildBoard();

	private void Update()
	{
		//* Delete this (or use it as hint)
		if (Input.GetKeyDown(KeyCode.Space))
		{
			
		}
	}

	private void OnEnable() 
	{
		OnMatchAction += (type, count) => OnMatch(type, count);
	}

	private void OnDisable() 
	{
		OnMatchAction -= (type, count) => OnMatch(type, count);
	}

	

	private void BuildBoard() 
	{
		for (var y = 0; y < _rows.Length; y++)
		{
			for (var x = 0; x < _rows.Max(row => row.Tiles.Length); x++)
			{				
				var tile = GetTile(x, y);

				tile.X = x;
				tile.Y = y;

				tile.Type = _tileTypes[Random.Range(0, _tileTypes.Length)];

				tile.Button.onClick.AddListener(() => Select(tile));
			}
		}

		if (_ensureNoStartingMatches) StartCoroutine(EnsureNoStartingMatches());
	}

	public void OnMatch(TileType type, int count)
	{
		UpdateMovementUI();
		UpdateScoreUI(type, count);
	}

	private void UpdateMovementUI()
	{
		_movementCounterText.SetText($"{_movesRemaining}");

		if (_movesRemaining == 0 && ScoreCounter.Instance.Score < _scoreObjective)
		{
			GameOver();
		}
	}

	private void UpdateScoreUI(TileType type, int count)
	{
		var newScore = ScoreCounter.Instance.Score += count * type.Value;

		if (newScore >= _scoreObjective)
		{
			Win();
		}
	}

	private IEnumerator EnsureNoStartingMatches()
	{
		var wait = new WaitForEndOfFrame();

		while (TileMatrixUtility.FindBestMatch(Matrix) != null)
		{
			Shuffle();
			yield return wait;
		}
	}
	private Tile GetTile(int x, int y) => _rows[y].Tiles[x];

	private Tile[] GetTiles(IList<TileData> tileData)
	{
		var length = tileData.Count;

		var tiles = new Tile[length];

		for (var i = 0; i < length; i++) tiles[i] = GetTile(tileData[i].X, tileData[i].Y);

		return tiles;
	}

	private void AutoMatch()
	{
		var bestMove = TileMatrixUtility.FindBestMove(Matrix);

		if (bestMove != null)
		{
			Select(GetTile(bestMove.X1, bestMove.Y1));
			Select(GetTile(bestMove.X2, bestMove.Y2));
		}
	}

	private async void Select(Tile tile)
	{
		if (_isSwapping || _isMatching || _isShuffling) return;

		if (!_selection.Contains(tile))
		{
			if (_selection.Count > 0)
			{
				if (Math.Abs(tile.X - _selection[0].X) == 1 && Math.Abs(tile.Y - _selection[0].Y) == 0
					|| Math.Abs(tile.Y - _selection[0].Y) == 1 && Math.Abs(tile.X - _selection[0].X) == 0)
					_selection.Add(tile);
			}
			else
			{
				_selection.Add(tile);
			}
		}

		if (_selection.Count < 2) return;

		await SwapAsync(_selection[0], _selection[1]);

		if (!await TryMatchAsync()) await SwapAsync(_selection[0], _selection[1]);

		// Check if there's new matches because of ours
		while (TileMatrixUtility.FindBestMatch(Matrix) != null)
		{
			if (!await TryMatchAsync()) await SwapAsync(_selection[0], _selection[1]);
		}

		_selection.Clear();
	}

	private async Task SwapAsync(Tile tile1, Tile tile2)
	{
		_isSwapping = true;

		_movesRemaining--;

		var icon1 = tile1.Icon;
		var icon2 = tile2.Icon;

		var icon1Transform = icon1.transform;
		var icon2Transform = icon2.transform;

		icon1Transform.SetParent(_swappingOverlay);
		icon2Transform.SetParent(_swappingOverlay);

		icon1Transform.SetAsLastSibling();
		icon2Transform.SetAsLastSibling();

		var sequence = DOTween.Sequence();

		sequence.Join(icon1Transform.DOMove(icon2Transform.position, _tweenDuration)
				.SetEase(Ease.OutBack))
				.Join(icon2Transform.DOMove(icon1Transform.position, _tweenDuration)
				.SetEase(Ease.OutBack));

		await sequence.Play()
					  .AsyncWaitForCompletion();

		icon1Transform.SetParent(tile2.transform);
		icon2Transform.SetParent(tile1.transform);

		tile1.Icon = icon2;
		tile2.Icon = icon1;

		var tile1Item = tile1.Type;

		tile1.Type = tile2.Type;

		tile2.Type = tile1Item;

		_isSwapping = false;
	}

	private async Task<bool> TryMatchAsync() // Intentar con Corutinas
	{
		var didMatch = false;

		_isMatching = true;

		var match = TileMatrixUtility.FindBestMatch(Matrix);

		while (match != null)
		{
			didMatch = true;

			var tiles = GetTiles(match.Tiles);

			var deflateSequence = DOTween.Sequence();

			foreach (var tile in tiles)
			{
				deflateSequence.Join(tile.Icon.transform.DOScale(Vector3.zero, _tweenDuration)
							   .SetEase(Ease.InBack));
			}

			audioSource.PlayOneShot(_matchSound);

			await deflateSequence.Play()
								 .AsyncWaitForCompletion();

			

			var inflateSequence = DOTween.Sequence();

			foreach (var tile in tiles)
			{
				tile.Type = _tileTypes[Random.Range(0, _tileTypes.Length)];

				inflateSequence.Join(tile.Icon.transform.DOScale(Vector3.one, _tweenDuration)
							   .SetEase(Ease.OutBack));
			}

			await inflateSequence.Play().AsyncWaitForCompletion();

			OnMatchAction?.Invoke(Array.Find(_tileTypes, tileType => tileType.Id == match.TypeId), match.Tiles.Length);

			match = TileMatrixUtility.FindBestMatch(Matrix);

			match = null;
		}

		if (didMatch) audioSource.PlayOneShot(_endMatchSound);

		_isMatching = false;

		return didMatch;
	}

	private void Shuffle() // Shuffle all tiles
	{
		_isShuffling = true;

		foreach (var row in _rows)
			foreach (var tile in row.Tiles)
				tile.Type = _tileTypes[Random.Range(0, _tileTypes.Length)];

		_isShuffling = false;
	}

	private async void GameOver()
	{
		var inflateSequence = DOTween.Sequence();

		inflateSequence.Join(_gameOverScreen.transform.DOScale(Vector3.one, _tweenDuration)
					   .SetEase(Ease.InBounce));

		await inflateSequence.Play().AsyncWaitForCompletion();
	}

	private async void Win()
	{

		var inflateSequence = DOTween.Sequence();

		inflateSequence.Join(_winScreen.transform.DOScale(Vector3.one, _tweenDuration)
					   .SetEase(Ease.InBounce));

		await inflateSequence.Play().AsyncWaitForCompletion();
	}
}

