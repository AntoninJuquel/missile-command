using System;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour, IHit
{
    public static readonly List<Building> Buildings = new List<Building>();
    public static Action<Building> OnBuildingDestroyed;
    [SerializeField] private int lives;

    protected virtual void Awake()
    {
        Buildings.Add(this);
    }

    public void TakeHit()
    {
        lives--;
        if (lives == 0) Die();
    }

    public virtual void Die()
    {
        OnBuildingDestroyed?.Invoke(this);
        Buildings.Remove(this);
        Destroy(gameObject);
    }
}