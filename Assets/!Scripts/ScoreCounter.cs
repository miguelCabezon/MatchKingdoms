using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class ScoreCounter : MonoBehaviour
{
    // THIS SHOULD BE AN EVENT BUS
    public static ScoreCounter Instance { get; private set; }

    private int _score;

    public int Score
    {
        get => _score;

        set
        {
            if (_score == value) return;

            _score = value;

            _scoreText.SetText($"{_score}");
        }
    }

    [SerializeField] private TMP_Text _scoreText;

    private void Awake() => Instance = this;
}
