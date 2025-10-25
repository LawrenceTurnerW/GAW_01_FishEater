using UnityEngine;

/// <summary>
/// ゲーム全体を管理し、プレイヤーと敵の衝突を検出
/// </summary>
public class GameManager : MonoBehaviour {
    [SerializeField] private PlayerFish _playerFish;

    private void Start() {
        if (_playerFish == null) {
            _playerFish = FindObjectOfType<PlayerFish>();
        }
    }
}