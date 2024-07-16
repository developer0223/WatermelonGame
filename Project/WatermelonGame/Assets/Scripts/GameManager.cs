// System
using System;
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.UI;

// Project
// Alias

// https://suika-game.app/ko
// https://namu.wiki/w/%EC%88%98%EB%B0%95%EA%B2%8C%EC%9E%84
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

    [Header("Popup")]
    [SerializeField] private GameObject gameOverPopup = null;

    [Header("Prefabs")]
    [SerializeField] private List<Sprite> fruitSpriteList = new List<Sprite>();
    [SerializeField] private List<GameObject> fruitPrefabList = new List<GameObject>();
    [SerializeField] private List<GameObject> defaultSpawnFruitsPrefabList = new List<GameObject>();

    [Header("Fruit Parent")]
    [SerializeField] private Transform fruitParentTransform = null;

    // public variables
    public bool isGameOver = false;

    // private variables
    private bool isSpawnable = true;

    private void Awake()
    {
        Instance = this;

        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(576, 1024, false);
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        if (isSpawnable && !isGameOver && Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            SpawnAndReloadFruit(worldPosition);

            StartCoroutine(SetNegativeSpawnableForSeconeds(1.0f));
        }
    }

    private IEnumerator SetNegativeSpawnableForSeconeds(float seconds)
    {
        isSpawnable = false;
        yield return new WaitForSeconds(seconds);
        isSpawnable = true;
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
        switch (fruitType)
        {
            case FruitType.Cherry:
                AddScore(1);
                break;

            case FruitType.Strawberry:
                AddScore(3);
                break;

            default:
                int fruitScore = (int)fruitType + 1;
                int addSumScore = fruitScore * (fruitScore + 1) / 2;
                AddScore(addSumScore);
                break;
        }

        FruitType nextType = fruitType.GetNextEnumType();
        SpawnFruit(nextType, worldPosition);
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        isSpawnable = false;

        gameOverPopup.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        for (int i = fruitParentTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(fruitParentTransform.GetChild(i).gameObject);
        }

        waterMelonScore = 0;

        spawnedIndex = 0;
        currentFruitType = GetRandomDefaultFruitType();
        nextFruitType = GetRandomDefaultFruitType();

        InvalidateCurrentNextFruitUI();
        InvalidateScoreUI();

        isGameOver = false;
        StartCoroutine(SetNegativeSpawnableForSeconeds(1.0f));

        gameOverPopup.gameObject.SetActive(false);
    }

    private void InvalidateCurrentNextFruitUI()
    {
        img_current_fruit.sprite = GetFruitSpriteWithType(currentFruitType);
        img_next_fruit.sprite = GetFruitSpriteWithType(nextFruitType);
    }

    private void AddScore(int score)
    {
        this.waterMelonScore += score;
        InvalidateScoreUI();
    }

    private void InvalidateScoreUI()
    {
        txt_score.text = waterMelonScore.ToString();
    }

    private Fruit SpawnFruit(FruitType fruitType, Vector3 worldPosition)
    {
        GameObject prefab = Instantiate(GetFruitPrefabWithType(fruitType), worldPosition, Quaternion.identity);
        prefab.transform.SetParent(fruitParentTransform);

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
        int randomIndex = UnityEngine.Random.Range(0, defaultSpawnFruitsPrefabList.Count - 1);
        return (FruitType) enumValues.GetValue(randomIndex);
    }
}