using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{
    public static readonly HashSet<Turret> Turrets = new HashSet<Turret>();
    public bool Available { get; private set; }
    [SerializeField] private Missile missilePrefab;
    [SerializeField] private float reloadTime, missileSpeed;
    [SerializeField] private Gradient gradient;
    [SerializeField] private LayerMask layerToHit;

    private SpriteRenderer _sr;

    protected override void Awake()
    {
        base.Awake();
        Turrets.Add(this);
        _sr = GetComponent<SpriteRenderer>();
        Available = true;
    }

    private IEnumerator Reload()
    {
        Available = false;
        var elapsedTime = 0f;
        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            _sr.color = gradient.Evaluate((elapsedTime / reloadTime));
            yield return null;
        }

        _sr.color = gradient.Evaluate(1);
        Available = true;
        yield return null;
    }

    public void Shoot(out Missile missile, Vector3 targetPosition)
    {
        missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        missile.Setup(targetPosition, missileSpeed, layerToHit);
        StartCoroutine(Reload());
    }

    public override void Die()
    {
        base.Die();
        Turrets.Remove(this);
    }
}