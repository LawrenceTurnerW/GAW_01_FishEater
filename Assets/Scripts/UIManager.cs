using UnityEngine;
using TMPro;

/// <summary>
/// ゲーム画面のUI表示を管理
/// </summary>
public class UIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _sizeText;

    private PlayerFish _playerFish;
    private GameManager _gameManager;

    private void Start() {
        _playerFish = FindObjectOfType<PlayerFish>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        // スコア更新
        if (_scoreText != null && _playerFish != null) {
            _scoreText.text = $"Score: {_playerFish.GetCurrentScore()}";
        }

        // サイズ更新
        if (_sizeText != null && _playerFish != null) {
            _sizeText.text = $"Size: {_playerFish.GetCurrentSize():F2}";
        }

        // 時間更新（GameManager から取得）
        if (_timeText != null && _gameManager != null) {
            float remainingTime = _gameManager.GetRemainingTime();
            _timeText.text = $"Time: {remainingTime:F1}s";
        }
    }
}