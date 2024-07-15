// System
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.UI;

// Project
// Alias

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int waterMelonScore = 0;

    private int spawnedIndex = 0;
    private FruitType currentFruitType = FruitType.None;
    private FruitType nextFruitType = FruitType.None;

    [Header("UI")]
    [SerializeField] private Image img_current_fruit = null;
    [SerializeField] private Image img_next_fruit = null;
    [SerializeField] private Text txt_score = null;


    [SerializeField] private List<Sprite> fruitSpriteList = new List<Sprite>();
    [SerializeField] private List<GameObject> fruitPrefabList = new List<GameObject>();
    [SerializeField] private List<GameObject> defaultSpawnFruitsPrefabList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentFruitType = GetRandomDefaultFruitType();
        nextFruitType = GetRandomDefaultFruitType();

        InvalidateCurrentNextFruitUI();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.y = 2.50f;
            worldPosition.z = 0.00f;

            SpawnAndReloadFruit(worldPosition);
        }
    }

    public void SpawnAndReloadFruit(Vector3 worldPosition)
    {
        switch (currentFruitType)
        {
            case FruitType.Cherry:
                worldPosition = new Vector3(Mathf.Clamp(worldPosition.x, -2.28f, 2.28f), 2.5f, 0);
                break;

            case FruitType.Strawberry:
                worldPosition = new Vector3(Mathf.Clamp(worldPosition.x, -2.16f, 2.16f), 2.5f, 0);
                break;

            case FruitType.Grape:
                worldPosition = new Vector3(Mathf.Clamp(worldPosition.x, -2.00f, 2.00f), 2.5f, 0);
                break;
        }

        SpawnFruit(currentFruitType, worldPosition);

        currentFruitType = nextFruitType;
        nextFruitType = GetRandomDefaultFruitType();

        InvalidateCurrentNextFruitUI();
    }

    public void SpawnNextFruit(FruitType fruitType, Vector3 worldPosition)
    {
        //FruitType nextType = (FruitType) (((int)fruitType) + 1);
        FruitType nextType = fruitType.GetNextEnumType();
        SpawnFruit(nextType, worldPosition);
    }

    private void InvalidateCurrentNextFruitUI()
    {
        img_current_fruit.sprite = GetFruitSpriteWithType(currentFruitType);
        img_next_fruit.sprite = GetFruitSpriteWithType(nextFruitType);
    }

    private Fruit SpawnFruit(FruitType fruitType, Vector3 worldPosition)
    {
        GameObject prefab = Instantiate(GetFruitPrefabWithType(fruitType), worldPosition, Quaternion.identity);
        Fruit script = prefab.GetComponent<Fruit>();
        script.spawnedIndex = spawnedIndex++;

        return script;
    }

    private Sprite GetFruitSpriteWithType(FruitType fruitType) => fruitType switch
    {
        FruitType.Cherry => fruitSpriteList[(int)FruitType.Cherry],
        FruitType.Strawberry => fruitSpriteList[(int)FruitType.Strawberry],
        FruitType.Grape => fruitSpriteList[(int)FruitType.Grape],
        FruitType.Persimmon => fruitSpriteList[(int)FruitType.Persimmon],
        FruitType.Orange => fruitSpriteList[(int)FruitType.Orange],
        FruitType.Apple => fruitSpriteList[(int)FruitType.Apple],
        FruitType.Pear => fruitSpriteList[(int)FruitType.Pear],
        FruitType.Peach => fruitSpriteList[(int)FruitType.Peach],
        FruitType.Pineapple => fruitSpriteList[(int)FruitType.Pineapple],
        FruitType.Melon => fruitSpriteList[(int)FruitType.Melon],
        FruitType.WaterMelon => fruitSpriteList[(int)FruitType.WaterMelon],
        _ => null
    };

    private GameObject GetFruitPrefabWithType(FruitType fruitType) => fruitType switch
    {
        FruitType.Cherry => fruitPrefabList[(int)FruitType.Cherry],
        FruitType.Strawberry => fruitPrefabList[(int)FruitType.Strawberry],
        FruitType.Grape => fruitPrefabList[(int)FruitType.Grape],
        FruitType.Persimmon => fruitPrefabList[(int)FruitType.Persimmon],
        FruitType.Orange => fruitPrefabList[(int)FruitType.Orange],
        FruitType.Apple => fruitPrefabList[(int)FruitType.Apple],
        FruitType.Pear => fruitPrefabList[(int)FruitType.Pear],
        FruitType.Peach => fruitPrefabList[(int)FruitType.Peach],
        FruitType.Pineapple => fruitPrefabList[(int)FruitType.Pineapple],
        FruitType.Melon => fruitPrefabList[(int)FruitType.Melon],
        FruitType.WaterMelon => fruitPrefabList[(int)FruitType.WaterMelon],
        _ => null
    };

    private FruitType GetRandomDefaultFruitType()
    {
        Array enumValues = Enum.GetValues(typeof(FruitType));
        return (FruitType) enumValues.GetValue(UnityEngine.Random.Range(0, 2));
        //return (FruitType) enumValues.GetValue(UnityEngine.Random.Range(0, enumValues.Length));
    }
}