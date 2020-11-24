using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    public Transform gameTile;
    public static float tileZ;
    public static float tileX;
    public string[] obstacleNames;
    public string[] pickupNames;
    public Transform fog;
    private float _pickupProbability;
    private float _specialPickupDivider;

    private void Awake()
    {
        tileZ = gameTile.GetComponent<Collider>().bounds.extents.z * 2;
        tileX = gameTile.GetComponent<Collider>().bounds.extents.x * 2;
    }

    public void StartSetUp(float tileSpawnObstaclesProbability, float obstacleSpawnProbability, float pickupProbability, float specialPickupDivider, int startAfterTile)
    {
        EmptyObstacles();
        _pickupProbability = pickupProbability;
        _specialPickupDivider = specialPickupDivider;
        if (transform.position.z > startAfterTile * tileZ)
        {
            int hole = Random.Range(-1, 2);
            for (int i = -1; i < 2; i++)
            {
                if (i != hole && Random.Range(0f, 1f) < tileSpawnObstaclesProbability && Random.Range(0f, 1f) < obstacleSpawnProbability)
                {
                    PlaceObject(i, obstacleNames[Random.Range(0, obstacleNames.Length)]);
                }
                else if (Random.Range(0f, 1f) < pickupProbability)
                {
                    PlaceObject(i, pickupNames[Random.Range(0, pickupNames.Length)]);
                }
            }
        }
    }

    private void PlaceObject(int i, string objectName)
    {
        var spawnedObject = ObjectPooler.Instance.SpawnFromPool(objectName);
        spawnedObject.parent = transform;
        spawnedObject.localPosition = new Vector3(
            i,
            0.5f,
            0
        );
        var obstacle = spawnedObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            obstacle.specialPickUp.gameObject.SetActive(Random.Range(0f, 1f) < _pickupProbability/_specialPickupDivider);
        }
    }

    private void EmptyObstacles()
    {
        foreach (Transform obstacle in transform)
        {
            if (obstacle.CompareTag("Obstacle") || obstacle.CompareTag("Coin"))
            {
                obstacle.gameObject.SetActive(false);
            }
        }
    }
}