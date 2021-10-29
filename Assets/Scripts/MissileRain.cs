using System.Collections;
using UnityEngine;

public class MissileRain : MonoBehaviour
{
    [SerializeField] private int maxMissile;
    [SerializeField] private float shootRate, missileSpeed;
    [SerializeField] private Missile missilePrefab;
    [SerializeField] private LayerMask layerToHit;

    private void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1f / shootRate);
            var missileNum = Random.Range(1, maxMissile);

            var buildings = Building.Buildings;

            for (var i = 0; i < missileNum; i++)
            {
                var target = buildings[Random.Range(0, buildings.Count)];
                var spawnPos = new Vector2(Random.Range(-10, 10), 10);
                var missile = Instantiate(missilePrefab, spawnPos, Quaternion.identity);
                missile.Setup(target.transform.position, missileSpeed, layerToHit);
            }
        }
    }
}