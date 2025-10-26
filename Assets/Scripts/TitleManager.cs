using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面を管理
/// </summary>
public class TitleManager : MonoBehaviour {
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _subtitleText;
    [SerializeField] private TextMeshProUGUI _instructionText;

    private void Start() {
        // ボタンのクリックイベント登録
        if (_startButton != null) {
            _startButton.onClick.AddListener(OnStartClicked);
        }

        if (_quitButton != null) {
            _quitButton.onClick.AddListener(OnQuitClicked);
        }

        // タイムスケール戻す（念のため）
        Time.timeScale = 1f;
    }

    /// <summary>
    /// スタートボタンクリック時
    /// </summary>
    private void OnStartClicked() {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 終了ボタンクリック時
    /// </summary>
    private void OnQuitClicked() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}