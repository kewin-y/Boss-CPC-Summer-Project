using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class BackgroundTiler : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundTiles = new GameObject[3];
    [SerializeField] private float width;

    private SpriteRenderer tileSpriteRenderer;
    private int currentTile;
    private Dictionary<int, int> Left = new Dictionary<int, int>();
    private Dictionary<int, int> Right = new Dictionary<int, int>();

    void Start()
    {
        SetUpDictionaries();
        currentTile = 1; // Start at tile 1 (2nd tile in array)

        // Get the width of 1 tile; they are all the same
        tileSpriteRenderer = backgroundTiles[0].GetComponent<SpriteRenderer>();
        width = tileSpriteRenderer.bounds.size.x;
    }

    private void SetUpDictionaries()
    {
        Left.Add(0, 2);
        Left.Add(1, 0);
        Left.Add(2, 1);

        Right.Add(0, 1);
        Right.Add(1, 2);
        Right.Add(2, 0);
    }

    public void MoveTileRight()
    {
        var leftTile = Left.GetValueOrDefault(currentTile);
        backgroundTiles[leftTile].transform.position = new(backgroundTiles[leftTile].transform.position.x + 3 * width, 0);
        currentTile = Right.GetValueOrDefault(currentTile);
    }

    public void MoveTileLeft()
    {
        var rightTile = Right.GetValueOrDefault(currentTile);
        backgroundTiles[rightTile].transform.position = new(backgroundTiles[rightTile].transform.position.x - 3 * width, 0);
        currentTile = Left.GetValueOrDefault(currentTile);
    }

    public void ResetTiles()
    {
        currentTile = 1;
        backgroundTiles[0].transform.position = new(-width, 0);
        backgroundTiles[1].transform.position = new(0, 0);
        backgroundTiles[2].transform.position = new(width, 0);
    }
}
