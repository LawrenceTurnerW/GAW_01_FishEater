using UnityEngine;

/// <summary>
/// 敵魚を画面左右から無限にスポーンさせる
/// ズーム値に応じて動的にスポーン位置を計算
/// プレイヤーサイズに応じて敵のサイズも調整
/// </summary>
public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnIntervalMin = 0.5f;
    [SerializeField] private float _spawnIntervalMax = 2.0f;

    // 敵のサイズ設定（複数の段階）
    [SerializeField] private float[] _enemySizesStage1 = new float[] { 0.3f, 0.5f, 0.8f, 1.2f, 1.5f };
    [SerializeField] private float[] _enemySizesStage2 = new float[] { 0.8f, 1.2f, 1.5f, 2.0f, 2.5f };
    [SerializeField] private float[] _enemySizesStage3 = new float[] { 1.5f, 2.0f, 2.5f, 3.5f, 4.0f };
    [SerializeField] private float[] _enemySizesStage4 = new float[] { 2.5f, 3.5f, 4.0f, 5.0f, 6.0f };

    // ステージの閾値
    [SerializeField] private float _stage2Threshold = 2.0f;
    [SerializeField] private float _stage3Threshold = 4.0f;
    [SerializeField] private float _stage4Threshold = 6.0f;

    private float _nextSpawnTime = 0f;
    private Transform _spawnParent;
    private CameraController _cameraController;
    private PlayerFish _playerFish;

    private void Start() {
        // 敵を管理するための親オブジェクトを作成
        GameObject enemyContainer = new GameObject("EnemyContainer");
        _spawnParent = enemyContainer.transform;

        _cameraController = FindObjectOfType<CameraController>();
        _playerFish = FindObjectOfType<PlayerFish>();

        _nextSpawnTime = Time.time + Random.Range(_spawnIntervalMin, _spawnIntervalMax);
    }

    private void Update() {
        // スポーン時間になったら敵を生成
        if (Time.time >= _nextSpawnTime) {
            SpawnEnemy();
            _nextSpawnTime = Time.time + Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        }
    }

    /// <summary>
    /// プレイヤーのサイズに応じた敵のサイズ配列を取得
    /// </summary>
    private float[] GetCurrentEnemySizes() {
        if (_playerFish == null)
            return _enemySizesStage1;

        float playerSize = _playerFish.GetCurrentSize();

        if (playerSize >= _stage4Threshold)
            return _enemySizesStage4;
        else if (playerSize >= _stage3Threshold)
            return _enemySizesStage3;
        else if (playerSize >= _stage2Threshold)
            return _enemySizesStage2;
        else
            return _enemySizesStage1;
    }

    private void SpawnEnemy() {
        // カメラのズームに応じたスポーン範囲を計算
        float screenWidth = 10f;  // デフォルト値
        float screenHeight = 10f;

        if (_cameraController != null) {
            screenWidth = _cameraController.GetScreenWidth() / 2f;
            screenHeight = _cameraController.GetScreenHeight() / 2f;
        }

        // ランダムに左右を決定
        bool spawnFromLeft = Random.value > 0.5f;

        // スポーン位置を決定（画面端の少し外側）
        float spawnX = spawnFromLeft ? -screenWidth - 1f : screenWidth + 1f;
        float spawnY = Random.Range(-screenHeight, screenHeight);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        // 移動方向を決定
        Vector2 moveDirection = spawnFromLeft ? Vector2.right : Vector2.left;

        // プレイヤーサイズに応じた敵サイズ配列を取得
        float[] currentEnemySizes = GetCurrentEnemySizes();

        // ランダムにサイズを決定
        float randomSize = currentEnemySizes[Random.Range(0, currentEnemySizes.Length)];

        // 敵を生成
        GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, _spawnParent);
        EnemyFish enemyFish = newEnemy.GetComponent<EnemyFish>();

        if (enemyFish != null) {
            enemyFish.Initialize(randomSize, moveDirection);
        }
    }
}