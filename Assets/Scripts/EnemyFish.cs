using UnityEngine;

/// <summary>
/// 敵魚の動作を管理
/// </summary>
public class EnemyFish : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _destroyBoundaryX = 60f;

    private Vector2 _moveDirection = Vector2.zero;
    private float _fishSize = 1f;
    private int _scoreValue = 0;

    /// <summary>
    /// 敵魚を初期化
    /// </summary>
    public void Initialize(float size, Vector2 spawnDirection) {
        _fishSize = size;
        _moveDirection = spawnDirection.normalized;

        // スコア値はサイズに応じて設定
        _scoreValue = Mathf.RoundToInt(size * 100f);

        // スプライトとコライダーのサイズを設定（スケール変更でコライダーも自動調整される）
        transform.localScale = Vector3.one * size;
    }

    private void FixedUpdate() {
        // 移動処理
        Vector3 newPosition = transform.position + (Vector3)_moveDirection * _moveSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;

        // 画面外に出たら削除
        if (Mathf.Abs(newPosition.x) > _destroyBoundaryX) {
            Destroy(gameObject);
        }
    }

    public float GetSize() => _fishSize;
    public int GetScoreValue() => _scoreValue;
}