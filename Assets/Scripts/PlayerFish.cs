using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー魚の移動と成長を管理
/// </summary>
public class PlayerFish : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _worldBoundaryX = 50f;
    [SerializeField] private float _worldBoundaryY = 50f;

    private Vector2 _currentMovement = Vector2.zero;
    private float _currentSize = 1f;
    private int _currentScore = 0;

    private InputSystem_Actions _inputActions;

    private void Awake() {
        _inputActions = new InputSystem_Actions();
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

            // ワールド境界内に制限
            newPosition.x = Mathf.Clamp(newPosition.x, -_worldBoundaryX, _worldBoundaryX);
            newPosition.y = Mathf.Clamp(newPosition.y, -_worldBoundaryY, _worldBoundaryY);

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
                GameOver();
            }
        }
    }

    private void GameOver() {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;  // ゲームを一時停止
    }

    public float GetCurrentSize() => _currentSize;
    public int GetCurrentScore() => _currentScore;
    public Vector3 GetPosition() => transform.position;
}