using UnityEngine;
using TMPro;

public class BoardView : MonoBehaviour
{
    [SerializeField]
    private EventBus _scoreUpdateEventBus;
    [SerializeField]
    private EventBus _movementPerformedEventBus;
    [SerializeField]
    private TMP_Text _scoreText = null;
    [SerializeField]
    private TMP_Text _movement = null;


    private void Awake()
    {
        _scoreText.text = "0";

        // _scoreUpdateEventBus.Event += ScoreUpdate;
    }

    private void OnDestroy()
    {
        // _scoreUpdateEventBus.Event -= ScoreUpdate;
    }

    private void ScoreUpdate(int scoreToAdd)
    {

    }

    private void MovementUpdate()
    {

    }
}
