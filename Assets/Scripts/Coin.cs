using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
}
