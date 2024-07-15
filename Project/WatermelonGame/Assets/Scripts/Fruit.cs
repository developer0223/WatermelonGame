// System
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.UI;

// Project
// Alias

public class Fruit : MonoBehaviour
{
    public int spawnedIndex = -1;
    public FruitType fruitType = FruitType.None;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != Tag.Fruit)
            return;

        Fruit otherScript = collision.gameObject.GetComponent<Fruit>();
        if (otherScript.fruitType == this.fruitType)
        {
            if (otherScript.spawnedIndex > this.spawnedIndex)
                GameManager.Instance.SpawnNextFruit(fruitType, transform.position);

            Destroy(gameObject);
        }
    }
}