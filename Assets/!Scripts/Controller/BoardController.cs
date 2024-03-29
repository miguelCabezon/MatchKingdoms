// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using MVC.Model;

// namespace MVC.Controller
// {
//     public class BoardController
//     {
//         private BoardModel _model;
//         private bool _isShuffling = false;

//         public event Action<TileModel> OnTileCreated = delegate (TileModel model) { };
//         public event Action<TileModel> OnTileDestroyed = delegate (TileModel model) { };
//         public event Action<TileModel> OnMovementPerformed = delegate { };
//         public event Action<TileModel> OnScoreUpdated = delegate { };
//         public event Action<TileModel> OnResourcesGathered = delegate { };
//         public event Action<TileModel> OnResourcesRecollected = delegate { };
//         public event Action<TileModel> OnLevelWin = delegate { };
//         public event Action<TileModel> OnLevelFailed = delegate { };


//         // private void UpdateMovementUI()
//         // {
//         // 	_movementCounterText.SetText($"{_movesRemaining}");

//         // 	if (_movesRemaining == 0 && ScoreCounter.Instance.Score < _scoreObjective && _actualState == State.GAME)
//         // 	{
//         // 		OnLoseAction?.Invoke();
//         // 	}
//         // }

//         public BoardController()
//         {
//             _model = new BoardModel();
//         }



//         private void AutoMatch() //* CONTROLLER
//         {
//             var bestMove = TileMatrixUtility.FindBestMove(_model.Matrix);

//             if (bestMove != null)
//             {
//                 Select(GetTile(bestMove.X1, bestMove.Y1));
//                 Select(GetTile(bestMove.X2, bestMove.Y2));
//             }
//         }

//         private void Shuffle() // Shuffle all tiles
//         {
//             _isShuffling = true;

//             foreach (var row in _model._rows)
//                 foreach (var tile in row.Tiles)
//                     tile.Type = _model._tileTypes[Random.Range(0, model._tileTypes.Length)];

//             _isShuffling = false;
//         }

//         public static void Swap(int x1, int y1, int x2, int y2, TileModel[,] tiles)
//         {
//             var tile1 = tiles[x1, y1];

//             tiles[x1, y1] = tiles[x2, y2];

//             tiles[x2, y2] = tile1;
//         }

//         public static (TileModel[], TileModel[]) GetConnections(int originX, int originY, TileModel[,] tiles)
//         {
//             var origin = tiles[originX, originY];

//             var width = tiles.GetLength(0);
//             var height = tiles.GetLength(1);

//             var horizontalConnections = new List<TileModel>();
//             var verticalConnections = new List<TileModel>();

//             for (var x = originX - 1; x >= 0; x--)
//             {
//                 var other = tiles[x, originY];

//                 if (other.TypeId != origin.TypeId) break;

//                 horizontalConnections.Add(other);
//             }

//             for (var x = originX + 1; x < width; x++)
//             {
//                 var other = tiles[x, originY];

//                 if (other.TypeId != origin.TypeId) break;

//                 horizontalConnections.Add(other);
//             }

//             for (var y = originY - 1; y >= 0; y--)
//             {
//                 var other = tiles[originX, y];

//                 if (other.TypeId != origin.TypeId) break;

//                 verticalConnections.Add(other);
//             }

//             for (var y = originY + 1; y < height; y++)
//             {
//                 var other = tiles[originX, y];

//                 if (other.TypeId != origin.TypeId) break;

//                 verticalConnections.Add(other);
//             }

//             return (horizontalConnections.ToArray(), verticalConnections.ToArray());
//         }

//         public static Match FindBestMatch(TileModel[,] tiles)
//         {
//             var bestMatch = default(Match);

//             for (var y = 0; y < tiles.GetLength(1); y++)
//             {
//                 for (var x = 0; x < tiles.GetLength(0); x++)
//                 {
//                     var tile = tiles[x, y];

//                     var (h, v) = GetConnections(x, y, tiles);

//                     var match = new Match(tile, h, v);

//                     if (match.Score < 0) continue;

//                     if (bestMatch == null)
//                     {
//                         bestMatch = match;
//                     }
//                     else if (match.Score > bestMatch.Score) bestMatch = match;
//                 }
//             }

//             return bestMatch;
//         }

//         public static List<Match> FindAllMatches(TileModel[,] tiles)
//         {
//             var matches = new List<Match>();

//             for (var y = 0; y < tiles.GetLength(1); y++)
//             {
//                 for (var x = 0; x < tiles.GetLength(0); x++)
//                 {
//                     var tile = tiles[x, y];

//                     var (h, v) = GetConnections(x, y, tiles);

//                     var match = new Match(tile, h, v);

//                     if (match.Score > -1) matches.Add(match);
//                 }
//             }

//             return matches;
//         }

//         private static (int, int) GetDirectionOffset(byte direction) => direction switch
//         {
//             0 => (-1, 0), // left
//             1 => (0, -1), // bottom
//             2 => (1, 0),  // right
//             3 => (0, 1),  // top

//             _ => (0, 0),  // origin
//         };

//         public static Move FindMove(TileModel[,] tiles)
//         {
//             var tilesCopy = (TileModel[,])tiles.Clone();

//             var width = tilesCopy.GetLength(0);
//             var height = tilesCopy.GetLength(1);

//             for (var y = 0; y < height; y++)
//             {
//                 for (var x = 0; x < width; x++)
//                 {
//                     for (byte d = 0; d <= 3; d++)
//                     {
//                         var (offsetX, offsetY) = GetDirectionOffset(d);

//                         var x2 = x + offsetX;
//                         var y2 = y + offsetY;

//                         if (x2 < 0 || x2 > width - 1 || y2 < 0 || y2 > height - 1) continue;

//                         Swap(x, y, x2, y2, tilesCopy);

//                         if (FindBestMatch(tilesCopy) != null) return new Move(x, y, x2, y2);

//                         Swap(x2, y2, x, y, tilesCopy);
//                     }
//                 }
//             }

//             return null;
//         }

//         public static Move FindBestMove(TileModel[,] tiles)
//         {
//             var tilesCopy = (TileModel[,])tiles.Clone();

//             var width = tilesCopy.GetLength(0);
//             var height = tilesCopy.GetLength(1);

//             var bestScore = int.MinValue;

//             var bestMove = default(Move);

//             for (var y = 0; y < height; y++)
//             {
//                 for (var x = 0; x < width; x++)
//                 {
//                     for (byte d = 0; d <= 3; d++)
//                     {
//                         var (offsetX, offsetY) = GetDirectionOffset(d);

//                         var x2 = x + offsetX;
//                         var y2 = y + offsetY;

//                         if (x2 < 0 || x2 > width - 1 || y2 < 0 || y2 > height - 1) continue;

//                         Swap(x, y, x2, y2, tilesCopy);

//                         var match = FindBestMatch(tilesCopy);

//                         if (match != null && match.Score > bestScore)
//                         {
//                             bestMove = new Move(x, y, x2, y2);

//                             bestScore = match.Score;
//                         }

//                         Swap(x, y, x2, y2, tilesCopy);
//                     }
//                 }
//             }

//             return bestMove;
//         }

//         public async void Select(TileView tile) //* CONTROLLER
//         {

//             if (_isSwapping || _isMatching || _isShuffling) return;

//             if (!_selection.Contains(tile))
//             {
//                 if (_selection.Count > 0)
//                 {
//                     if (Math.Abs(tile.X - _selection[0].X) == 1 && Math.Abs(tile.Y - _selection[0].Y) == 0
//                         || Math.Abs(tile.Y - _selection[0].Y) == 1 && Math.Abs(tile.X - _selection[0].X) == 0)
//                         _selection.Add(tile);
//                 }
//                 else
//                 {
//                     _selection.Add(tile);
//                 }
//             }

//             if (_selection.Count < 2) return;

//             await SwapAsync(_selection[0], _selection[1]);

//             if (!await TryMatchAsync()) await SwapAsync(_selection[0], _selection[1]);

//             // Check if there's new matches because of ours
//             while (TileMatrixUtility.FindBestMatch(Matrix) != null)
//             {
//                 if (!await TryMatchAsync()) await SwapAsync(_selection[0], _selection[1]);
//             }

//             _audioSource.PlayOneShot(_endMatchSound);

//             _selection.Clear();
//         }

//         private async Task SwapAsync(TileView tile1, TileView tile2) //* CONTROLLER
// 	{
// 		_isSwapping = true;

// 		_movesRemaining--;

// 		var icon1 = tile1.Icon;
// 		var icon2 = tile2.Icon;

// 		var icon1Transform = icon1.transform;
// 		var icon2Transform = icon2.transform;

// 		icon1Transform.SetParent(_swappingOverlay);
// 		icon2Transform.SetParent(_swappingOverlay);

// 		icon1Transform.SetAsLastSibling();
// 		icon2Transform.SetAsLastSibling();

// 		var sequence = DOTween.Sequence();

// 		sequence.Join(icon1Transform.DOMove(icon2Transform.position, _tweenDuration)
// 				.SetEase(Ease.OutBack))
// 				.Join(icon2Transform.DOMove(icon1Transform.position, _tweenDuration)
// 				.SetEase(Ease.OutBack));

// 		await sequence.Play()
// 					  .AsyncWaitForCompletion();

// 		icon1Transform.SetParent(tile2.transform);
// 		icon2Transform.SetParent(tile1.transform);

// 		tile1.Icon = icon2;
// 		tile2.Icon = icon1;

// 		var tile1Item = tile1.Type;

// 		tile1.Type = tile2.Type;

// 		tile2.Type = tile1Item;

// 		_isSwapping = false;
// 	}

// 	private async Task<bool> TryMatchAsync() // Intentar con Corutinas //* CONTROLLER
// 	{
// 		var didMatch = false;

// 		_isMatching = true;

// 		var match = TileMatrixUtility.FindBestMatch(Matrix);

// 		while (match != null)
// 		{
// 			didMatch = true;

// 			var tiles = GetTiles(match.Tiles);

// 			var deflateSequence = DOTween.Sequence();

// 			foreach (var tile in tiles)
// 			{
// 				deflateSequence.Join(tile.Icon.transform.DOScale(Vector3.zero, _tweenDuration)
// 							   .SetEase(Ease.InBack));
// 			}

// 			_audioSource.PlayOneShot(_matchSound);

// 			await deflateSequence.Play()
// 								 .AsyncWaitForCompletion();

			

// 			var inflateSequence = DOTween.Sequence();

// 			foreach (var tile in tiles)
// 			{
// 				tile.Type = _tileTypes[Random.Range(0, _tileTypes.Length)];

// 				inflateSequence.Join(tile.Icon.transform.DOScale(Vector3.one, _tweenDuration)
// 							   .SetEase(Ease.OutBack));
// 			}

// 			await inflateSequence.Play().AsyncWaitForCompletion();

// 			OnMatchAction?.Invoke(Array.Find(_tileTypes, tileType => tileType.Id == match.TypeId), match.Tiles.Length);

// 			match = TileMatrixUtility.FindBestMatch(Matrix);

// 			match = null;
// 		}

// 		_isMatching = false;

// 		return didMatch;
// 	}

//         private async void GameOver() //* CONTROLLER/VIEW
//         {
//             _actualState = GameState.LOSE;

//             var inflateSequence = DOTween.Sequence();

//             inflateSequence.Join(_gameOverScreen.transform.DOScale(Vector3.one, _tweenDuration)
//                            .SetEase(Ease.InBounce));

//             _audioSource.PlayOneShot(_secretSound);

//             await inflateSequence.Play().AsyncWaitForCompletion();
//         }

//         private async void Win() //* CONTROLLER/VIEW
//         {
//             _actualState = GameState.WIN;

//             _finalWoodText.text = _woodAmountToTown.ToString();
//             _finalFoodText.text = _foodAmountToTown.ToString();
//             _finalCoinText.text = _coinAmountToTown.ToString();

//             var inflateSequence = DOTween.Sequence();

//             inflateSequence.Join(_winScreen.transform.DOScale(Vector3.one, _tweenDuration)
//                            .SetEase(Ease.InBounce));

//             _audioSource.PlayOneShot(_winSound);

//             await inflateSequence.Play().AsyncWaitForCompletion();
//         }
//     }
// }
