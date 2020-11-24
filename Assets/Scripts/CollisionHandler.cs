using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public static event Action<Collider> ObstacleHit;
    public static event Action<int> CoinHit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ObstacleHit?.Invoke(other);
        }

        if (other.CompareTag("Coin"))
        {
            var coin = other.GetComponent<Coin>();
            CoinHit?.Invoke(coin.value);
            other.gameObject.SetActive(false);
            AudioManager.Instance.PlaySfx(coin.gem ? "Gem" : "Coin");
            FXManager.Instance.PlayFX(coin.gem ? "GemFX" : "CoinFX", coin.transform.position);
        }
    }
    
}
