using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤー魚の移動と成長を管理
/// </summary>
public class PlayerFish : MonoBehaviour {
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _worldBoundaryX = 1f;
    [SerializeField] private float _worldBoundaryY = 10f;


    private Vector2 _currentMovement = Vector2.zero;

    // プレイヤーの大きさ
    private float _currentSize = 0;

    // スコア(後で別に移すかも)
    private int _currentScore = 0;

    private CircleCollider2D _collider;
    private InputSystem_Actions _inputActions;

    private void Awake() {
        _inputActions = new InputSystem_Actions();
        _collider = GetComponent<CircleCollider2D>();
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

    // 敵を食べた時に呼び出す
    public void EatEnemy() {
    }

    private void UpdateSize() {
    }
}