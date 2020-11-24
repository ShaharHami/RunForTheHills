using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    public bool gem;
    public Transform pickUpEffect;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
}
