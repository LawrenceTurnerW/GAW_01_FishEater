using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバーシーンを管理
/// </summary>
public class GameOverManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private TextMeshProUGUI _finalSizeText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;

    private void Start() {
        // PlayerPrefs から最終結果を取得
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        float finalSize = PlayerPrefs.GetFloat("FinalSize", 1f);

        // UI を更新
        if (_finalScoreText != null) {
            _finalScoreText.text = $"Final Score: {finalScore}";
        }

        if (_finalSizeText != null) {
            _finalSizeText.text = $"Final Size: {finalSize:F2}";
        }

        // ボタンイベント登録
        if (_restartButton != null) {
            _restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (_menuButton != null) {
            _menuButton.onClick.AddListener(OnMenuClicked);
        }
    }

    /// <summary>
    /// リスタートボタンクリック時
    /// </summary>
    private void OnRestartClicked() {
        Time.timeScale = 1f;  // タイムスケール戻す
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// メニューボタンクリック時
    /// </summary>
    private void OnMenuClicked() {
        Time.timeScale = 1f;  // タイムスケール戻す
        // メニューシーンがあればロードする
        // SceneManager.LoadScene("MenuScene");
        Debug.Log("Menu button clicked - TODO: Load menu scene");
    }
}