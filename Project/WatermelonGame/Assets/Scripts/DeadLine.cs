// System
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.UI;

// Project
// Alias

public class DeadLine : MonoBehaviour
{
    private float gameOverTime = 3.0f;

    private List<Fruit> overlappedFruitsList = new List<Fruit>();

    private void Start()
    {
        StartCoroutine(Co_CheckGameOver());
    }

    private IEnumerator Co_CheckGameOver()
    {
        float elapsedTime = 0;
        while (Application.isPlaying)
        {
            if (overlappedFruitsList.Count > 0)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > gameOverTime)
                {
                    GameManager.Instance.GameOver();
                }
            }
            else
            {
                elapsedTime = 0;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Fruit))
        {
            Fruit overlappedFruit = collision.GetComponent<Fruit>();
            overlappedFruitsList.Add(overlappedFruit);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Fruit))
        {
            Fruit exitedFruit = collision.GetComponent<Fruit>();
            overlappedFruitsList.Remove(exitedFruit);
        }
    }
}