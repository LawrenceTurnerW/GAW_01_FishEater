using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー魚の移動と成長を管理
/// </summary>
public class PlayerFish : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _baseBoundaryX = 50f;  // 基準となるX境界（ズーム10の場合）
    [SerializeField] private float _baseBoundaryY = 50f;  // 基準となるY境界（ズーム10の場合）

    private Vector2 _currentMovement = Vector2.zero;
    private float _currentSize = 1f;
    private int _currentScore = 0;

    private InputSystem_Actions _inputActions;
    private CameraController _cameraController;

    private void Awake() {
        _inputActions = new InputSystem_Actions();
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void OnEnable() {
        _inputActions.Player.Enable();
    }

    private void OnDisable() {
        _inputActions.Player.Disable();
    }

    private void Update() {
        // Input System から移動入力を取得
        _currentMovement = _inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        // 移動処理
        if (_currentMovement.magnitude > 0) {
            Vector3 newPosition = transform.position + (Vector3)_currentMovement * _moveSpeed * Time.fixedDeltaTime;

            // カメラのズームに応じた動的な境界を計算
            float boundaryX = _baseBoundaryX;
            float boundaryY = _baseBoundaryY;

            if (_cameraController != null) {
                float screenWidth = _cameraController.GetScreenWidth() / 2f;
                float screenHeight = _cameraController.GetScreenHeight() / 2f;

                boundaryX = screenWidth * 0.95f;  // 画面端から少し内側
                boundaryY = screenHeight * 0.95f;
            }

            // 動的な境界内に制限
            newPosition.x = Mathf.Clamp(newPosition.x, -boundaryX, boundaryX);
            newPosition.y = Mathf.Clamp(newPosition.y, -boundaryY, boundaryY);

            transform.position = newPosition;
        }
    }

    /// <summary>
    /// 敵を食べた時に呼び出す
    /// </summary>
    public void EatEnemy(int scoreValue, float enemySize) {
        _currentScore += scoreValue;

        // スコアに応じてサイズを更新
        float newSize = 1f + (_currentScore / 100f);
        UpdateSize(newSize);

        Debug.Log($"Score: {_currentScore}, Size: {_currentSize:F2}");
    }

    private void UpdateSize(float newSize) {
        _currentSize = newSize;
        // スケール変更でビジュアルとコライダーが一緒に拡大される
        transform.localScale = Vector3.one * _currentSize;
    }

    /// <summary>
    /// トリガーコライダーに触れた時に呼び出される
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision) {
        EnemyFish enemy = collision.GetComponent<EnemyFish>();
        if (enemy != null) {
            // プレイヤーのサイズが敵より大きいかチェック
            if (_currentSize > enemy.GetSize()) {
                // 敵を食べる
                EatEnemy(enemy.GetScoreValue(), enemy.GetSize());

                // 敵を削除
                Destroy(enemy.gameObject);
            } else {
                // プレイヤーが敵より小さい場合、ゲームオーバー
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null) {
                    gameManager.GameOver();
                }
            }
        }
    }

    public float GetCurrentSize() => _currentSize;
    public int GetCurrentScore() => _currentScore;
    public Vector3 GetPosition() => transform.position;
}