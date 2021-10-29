using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform crosshair;
    private Camera _cam;
    private Missile _missile;
    private Vector3 _mousePosition;

    private void Awake()
    {
        Cursor.visible = false;
        _cam = Camera.main;
    }

    private void Update()
    {
        _mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        crosshair.position = _mousePosition;
        if (!Input.GetMouseButtonDown(0)) return;
        if (_missile) Detonate();
        else Shoot();
    }

    private Turret GetNearestTurret(Vector2 position)
    {
        Turret nearestTurret = null;
        var nearestDistance = Mathf.Infinity;
        foreach (var turret in Turret.Turrets)
        {
            var distance = Vector2.Distance(position, turret.transform.position);
            if (distance >= nearestDistance || !turret.Available) continue;

            nearestDistance = distance;
            nearestTurret = turret;
        }

        return nearestTurret;
    }

    private void Detonate()
    {
        _missile.Detonate();
        _missile = null;
    }

    private void Shoot()
    {
        var nearestTurret = GetNearestTurret(_mousePosition);

        if (!nearestTurret) return;

        nearestTurret.Shoot(out _missile, _mousePosition);
        _missile.OnDetonation += () => _missile = null;
    }
}