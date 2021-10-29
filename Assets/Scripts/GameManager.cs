using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Building.OnBuildingDestroyed += CheckGameOver;
    }

    private void CheckGameOver(Building building)
    {
        if (Building.Buildings.Count == 0) SceneManager.LoadScene("Game");
    }
}