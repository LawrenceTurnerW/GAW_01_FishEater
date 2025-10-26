using UnityEngine;

/// <summary>
/// プレイヤーの成長に応じてカメラをズームアウト
/// ズーム値に基づいて動的に座標を計算
/// </summary>
public class CameraController : MonoBehaviour {
    [SerializeField] private PlayerFish _playerFish;
    [SerializeField] private float _baseZoom = 10f;  // 初期ズーム値
    [SerializeField] private float _zoomMultiplier = 2f;  // サイズに対するズーム倍率
    [SerializeField] private float _zoomSmoothness = 5f;  // ズーム速度
    [SerializeField] private float _screenAspectRatio = 16f / 9f;  // 画面アスペクト比

    private Camera _camera;
    private float _targetZoom;
    private float _currentZoom;

    private void Start() {
        _camera = GetComponent<Camera>();

        if (_playerFish == null) {
            _playerFish = FindObjectOfType<PlayerFish>();
        }

        if (_camera == null) {
            Debug.LogError("Camera component not found!");
        }

        _targetZoom = _baseZoom;
        _currentZoom = _baseZoom;
    }

    private void Update() {
        if (_playerFish == null || _camera == null)
            return;

        // プレイヤーのサイズに応じてターゲットズームを計算
        float playerSize = _playerFish.GetCurrentSize();
        _targetZoom = _baseZoom + (playerSize - 1f) * _zoomMultiplier;

        // 現在のズームをターゲットに向けてスムーズに変更
        _currentZoom = Mathf.Lerp(
            _currentZoom,
            _targetZoom,
            _zoomSmoothness * Time.deltaTime
        );

        _camera.orthographicSize = _currentZoom;
    }

    /// <summary>
    /// 現在のズーム値に基づいた画面の幅を取得
    /// </summary>
    public float GetScreenWidth() {
        return _currentZoom * 2f * _screenAspectRatio;
    }

    /// <summary>
    /// 現在のズーム値に基づいた画面の高さを取得
    /// </summary>
    public float GetScreenHeight() {
        return _currentZoom * 2f;
    }

    /// <summary>
    /// 現在のズーム値を取得
    /// </summary>
    public float GetCurrentZoom() {
        return _currentZoom;
    }
}