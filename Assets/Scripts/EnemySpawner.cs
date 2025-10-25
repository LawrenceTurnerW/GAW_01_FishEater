using UnityEngine;

/// <summary>
/// 敵魚を画面左右から無限にスポーンさせる
/// </summary>
public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnIntervalMin = 0.5f;
    [SerializeField] private float _spawnIntervalMax = 2.0f;
    [SerializeField] private float _cameraHeight = 10f;
    [SerializeField] private float _cameraWidth = 17.77f;  // アスペクト比 16:9 の場合

    // 敵のサイズ設定
    [SerializeField] private float[] _enemySizes = new float[] { 0.3f, 0.5f, 0.8f, 1.2f, 1.5f };

    private float _nextSpawnTime = 0f;
    private Transform _spawnParent;

    private void Start() {
        // 敵を管理するための親オブジェクトを作成
        GameObject enemyContainer = new GameObject("EnemyContainer");
        _spawnParent = enemyContainer.transform;

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
        // ランダムに左右を決定
        bool spawnFromLeft = Random.value > 0.5f;

        // スポーン位置を決定
        float spawnX = spawnFromLeft ? -_cameraWidth : _cameraWidth;
        float spawnY = Random.Range(-_cameraHeight / 2f, _cameraHeight / 2f);
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