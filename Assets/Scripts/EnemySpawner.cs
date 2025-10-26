using UnityEngine;

/// <summary>
/// 敵魚を画面左右から無限にスポーンさせる
/// ズーム値に応じて動的にスポーン位置を計算
/// </summary>
public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnIntervalMin = 0.5f;
    [SerializeField] private float _spawnIntervalMax = 2.0f;

    // 敵のサイズ設定
    [SerializeField] private float[] _enemySizes = new float[] { 0.3f, 0.5f, 0.8f, 1.2f, 1.5f };

    private float _nextSpawnTime = 0f;
    private Transform _spawnParent;
    private CameraController _cameraController;

    private void Start() {
        // 敵を管理するための親オブジェクトを作成
        GameObject enemyContainer = new GameObject("EnemyContainer");
        _spawnParent = enemyContainer.transform;

        _cameraController = FindObjectOfType<CameraController>();

        _nextSpawnTime = Time.time + Random.Range(_spawnIntervalMin, _spawnIntervalMax);
    }

    private void Update() {
        // スポーン時間になったら敵を生成
        if (Time.time >= _nextSpawnTime) {
            SpawnEnemy();
            _nextSpawnTime = Time.time + Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        }
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

        // ランダムにサイズを決定
        float randomSize = _enemySizes[Random.Range(0, _enemySizes.Length)];

        // 敵を生成
        GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, _spawnParent);
        EnemyFish enemyFish = newEnemy.GetComponent<EnemyFish>();

        if (enemyFish != null) {
            enemyFish.Initialize(randomSize, moveDirection);
        }
    }
}