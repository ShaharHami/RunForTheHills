using UnityEngine;

public class LivesIndicator : MonoBehaviour
{
    public Transform heart;
    private Health _health;

    public void AddLife()
    {
        var newHeart = Instantiate(heart);
        newHeart.SetParent(transform);
    }
    public void LoseLife()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount-1).gameObject);
        }
    }
}
