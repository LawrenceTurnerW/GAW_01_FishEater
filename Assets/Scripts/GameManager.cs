using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理し、プレイヤーと敵の衝突を検出
/// </summary>
public class GameManager : MonoBehaviour {
    [SerializeField] private PlayerFish _playerFish;
    [SerializeField] private float _gameDuration = 60f;  // ゲーム時間（秒）

    private float _remainingTime;
    private bool _isGameOver = false;

    private void Start() {
        if (_playerFish == null) {
            _playerFish = FindObjectOfType<PlayerFish>();
        }

        _remainingTime = _gameDuration;
    }

    private void Update() {
        if (_isGameOver)
            return;

        // タイマーカウントダウン
        _remainingTime -= Time.deltaTime;

        // 時間切れ
        if (_remainingTime <= 0) {
            _remainingTime = 0;
            GameOver();
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver() {
        if (_isGameOver)
            return;

        _isGameOver = true;

        // プレイヤーの最終スコア・サイズを保存
        if (_playerFish != null) {
            PlayerPrefs.SetInt("FinalScore", _playerFish.GetCurrentScore());
            PlayerPrefs.SetFloat("FinalSize", _playerFish.GetCurrentSize());
        }

        // ゲームオーバーシーンへ遷移
        Time.timeScale = 1f;  // タイムスケール戻す
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// 残り時間を取得
    /// </summary>
    public float GetRemainingTime() => _remainingTime;

    /// <summary>
    /// ゲームオーバー状態を取得
    /// </summary>
    public bool IsGameOver() => _isGameOver;
}