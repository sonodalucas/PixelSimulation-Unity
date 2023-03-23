using System;
using TMPro;
using UnityEngine;

public class Grid
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    
    private float cellSize;
    private TextMeshPro[,] debugTextArray;
    private int[,] gridArray;
    private int height;
    private Vector3 originPosition;
    private int width;
    private GameObject parent;
    private bool showDebug = false;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[this.width, this.height];

        if (showDebug)
        {
            debugTextArray = new TextMeshPro[this.width, this.height];
            parent = new GameObject();
            parent.name = "Grid";

            for (var y = 0; y < gridArray.GetLength(1); y++)
            {
                for (var x = 0; x < gridArray.GetLength(0); x++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);


                    debugTextArray[x, y] = UtilityLibrary.CreateWorldText(gridArray[x, y].ToString(), parent.transform,
                        GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 4, Color.white,
                        VerticalAlignmentOptions.Middle, HorizontalAlignmentOptions.Center);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            if (showDebug) 
                debugTextArray[x, y].text = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE).ToString();
            
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{x = x, y = y});
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;

        GetGridPosition(worldPosition, out x, out y);

        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) return gridArray[x, y];

        return 0;
    }

    public int GetValue(Vector3 wordPosition)
    {
        int x, y;
        GetGridPosition(wordPosition, out x, out y);

        return GetValue(x, y);
    }

    public void AddValue(int x, int y, int value)
    {
        SetValue(x, y, GetValue(x, y) + value);
    }

    public void AddValue(Vector3 worldPosition, int value, int totalRange, int fullValueRange)
    {
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));
        
        GetGridPosition(worldPosition, out int originX, out int originY);

        for (int y = 0; y < totalRange; y++)
        {
            for (int x = 0; x < totalRange - y; x++)
            {
                int radius = x + y;
                int addValueAmount = value;
                if (radius > fullValueRange)
                {
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                }
                // Triangle to the upper right
                AddValue(originX + x, originY + y, addValueAmount);
                
                // Triangle to the upper left
                if (x != 0)
                    AddValue(originX - x, originY + y, addValueAmount);
                
                if (y != 0)
                { 
                    // Triangle to the bottom right
                    if (x != 0)
                        AddValue(originX + x, originY - y, addValueAmount);
                
                    // Triangle to the bottom left
                    AddValue(originX - x, originY - y, addValueAmount);
                }
            }
        }
    }
}