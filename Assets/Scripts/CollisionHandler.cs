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
            CoinHit?.Invoke(other.GetComponent<Coin>().value);
            other.gameObject.SetActive(false);
        }
    }
    
}
