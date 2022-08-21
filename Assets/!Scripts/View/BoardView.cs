using UnityEngine;
using TMPro;

//? THIS SHOULD BE THE ONLY VIEW IN GAME (?)
public class BoardView : MonoBehaviour //& Board View
{
    [SerializeField] private int _scoreObjective = 200;
	[SerializeField] private int _movesLimit = 10;
    [SerializeField]
    private EventBus _scoreUpdateEventBus;
    [SerializeField]
    private EventBus _movementPerformedEventBus;
    [SerializeField] private TMP_Text _movementCounterText;
    [SerializeField] private GameObject _gameOverScreen;
	[SerializeField] private GameObject _winScreen;
    [SerializeField]
    private TMP_Text _scoreText = null;
    [SerializeField]
    private TMP_Text _movement = null;


    private void Awake()
    {
        _scoreText.text = "0";

        
    }

    private void OnEnable()
    {
        // _scoreUpdateEventBus.Event += ScoreUpdate;
        _movementPerformedEventBus.Event += MovementUpdate;
    }

    private void OnDisable()
    {
        // _scoreUpdateEventBus.Event -= ScoreUpdate;
        _movementPerformedEventBus.Event -= MovementUpdate;
    }

    private void ScoreUpdate(int scoreToAdd)
    {

    }

    private void MovementUpdate()
    {
        // _movementCounterText.SetText($"{_movesRemaining}");
    }
}
