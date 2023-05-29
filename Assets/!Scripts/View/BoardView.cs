// using UnityEngine;
// using TMPro;
// using MVC.Controller;
// using MVC.Model;
// using System.Collections.Generic;
// using System.Collections;

// namespace MVC.View
// {
//     public class BoardView : MonoBehaviour //& Board View
//     {
//         // [SerializeField] private Vector2Int _boardSize = new Vector2Int(9, 9); //^For future iterations

//         private List<TileView> _tiles = new List<TileView>();
//         private BoardController _controller;
//         private List<IViewAnimation> _animations = new List<IViewAnimation>();

//         private bool _isAnimating => _animations.Count > 0;

//         private void Awake()
//         {
//             _controller = new BoardController();
//         }

//         private void OnEnable()
//         {
//             _controller.OnTileCreated += OnTileCreated;
//             _controller.OnTileDestroyed += OnTileDestroyed;

//             _controller.OnMovementPerformed += OnMovementPerformed;
//             _controller.OnScoreUpdated += OnScoreUpdated;
//             _controller.OnResourcesGathered += OnResourcesGathered; //~ Action to gathered resources
//             // _controller.OnResourcesRecollected += OnResourcesRecollected; //~ Action to "to town" resources
//             // _controller.OnLevelCompleted += OnLevelCompleted;
//             _controller.OnLevelWin += OnLevelWin;
//             _controller.OnLevelFailed += OnLevelFailed;
//         }

//         private void OnTileCreated(TileModel tile)
//         {
//             Debug.Log("TILE CREATED!");
//             _animations.Add(new CreateTileAnimation(tile.Position, tile.TypeId));
//             if (_animations.Count == 1)
//                 StartCoroutine(ProcessAnimations());
//         }

//         private void OnTileDestroyed(TileModel tile)
//         {
//             Debug.Log("TILE DESTROYED!");
//             // _animations.Add(new DestroyTileAnimation(tile.Position, tile.TypeId));
//             if (_animations.Count == 1)
//                 StartCoroutine(ProcessAnimations());
//         }

//         private void OnMovementPerformed()
//         {
//             Debug.Log("MOVEMENT PERFORMED!");
//             // _movementCounterText.SetText($"{_movesRemaining}"); 
//         }

//         private void OnScoreUpdated()
//         {
//             Debug.Log("SCORE UPDATED!");
//             // _movementCounterText.SetText($"{_movesRemaining}"); 
//         }

//         private void OnResourcesGathered()
//         {
//             Debug.Log("RESOURCES GATHERED!");
//             // _movementCounterText.SetText($"{_movesRemaining}"); 
//         }

//         private void OnResourcesRecollected()
//         {
//             Debug.Log("RESOURCES RECOLLECTED!");
//             // _movementCounterText.SetText($"{_movesRemaining}"); 
//         }

//         private void OnLevelWin()
//         {
//             Debug.Log("LEVEL WIN!");
//             // _movementCounterText.SetText($"{_movesRemaining}"); 
//         }

//         private void OnLevelFailed()
//         {
//             Debug.Log("LEVEL FAILED!");
//             // _movementCounterText.SetText($"{_movesRemaining}"); 
//         }


//         private IEnumerator ProcessAnimations()
//         {
//             while (_isAnimating)
//             {
//                 yield return _animations[0].PlayAnimation(this);
//                 _animations.RemoveAt(0);
//             }
//         }


//         // [SerializeField] private int _scoreObjective = 200;
//         // [SerializeField] private int _movesLimit = 10;
//         // [SerializeField]
//         // private EventBus _scoreUpdateEventBus;
//         // [SerializeField]
//         // private EventBus _movementPerformedEventBus;
//         // [SerializeField] private TMP_Text _movementCounterText;
//         // [SerializeField] private GameObject _gameOverScreen;
//         // [SerializeField] private GameObject _winScreen;
//         // [SerializeField]
//         // private TMP_Text _scoreText = null;
//         // [SerializeField]
//         // private TMP_Text _movement = null;


//         // private void Awake()
//         // {
//         //     _scoreText.text = "0";


//         // }
//     }
// }