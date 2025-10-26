using UnityEngine;

/// <summary>
/// 敵魚の動作を管理
/// ズーム値に応じて動的に消滅範囲を計算
/// </summary>
public class EnemyFish : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _baseDestroyBoundaryX = 60f;  // 基準となる消滅X座標（ズーム10の場合）
    [SerializeField] private float _spriteScaleBase = 0.1f;  // スプライトの基準スケール

    private Vector2 _moveDirection = Vector2.zero;
    private float _fishSize = 1f;
    private int _scoreValue = 0;
    private CameraController _cameraController;
    private Transform _spriteTransform;

    private void Start() {
        _cameraController = FindObjectOfType<CameraController>();
    }

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

        // 子オブジェクト「Sprite」のスケールを取得（初回のみ）
        if (_spriteTransform == null) {
            _spriteTransform = transform.Find("Sprite");
            if (_spriteTransform == null) {
                Debug.LogWarning($"[{gameObject.name}] 子オブジェクト \"Sprite\" が見つかりません");
                return;
            }
        }

        // 移動方向に応じてスプライトのスケール X を反転
        // 左移動（moveDirection.x < 0）の場合、スケールXを正にする（左向き）
        // 右移動（moveDirection.x > 0）の場合、スケールXを負にする（右向き）
        float spriteScaleX = _moveDirection.x < 0 ? _spriteScaleBase : -_spriteScaleBase;
        _spriteTransform.localScale = new Vector3(spriteScaleX, _spriteScaleBase, 1f);

        Debug.Log($"[{gameObject.name}] 移動方向: {_moveDirection}, Sprite Scale X: {spriteScaleX}");
    }

    private void FixedUpdate() {
        // 移動処理
        Vector3 newPosition = transform.position + (Vector3)_moveDirection * _moveSpeed * Time.fixedDeltaTime;
        transform.position = newPosition;

        // カメラのズームに応じた動的な消滅範囲を計算
        float destroyBoundaryX = _baseDestroyBoundaryX;

        if (_cameraController != null) {
            float screenWidth = _cameraController.GetScreenWidth() / 2f;
            // 画面端から少し外側で消滅するように設定
            destroyBoundaryX = screenWidth * 1.5f;
        }

        // 画面外に出たら削除
        if (Mathf.Abs(newPosition.x) > destroyBoundaryX) {
            Destroy(gameObject);
        }
    }

    public float GetSize() => _fishSize;
    public int GetScoreValue() => _scoreValue;
}