using System;
using TMPro;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    
    private TGridObject[,] gridArray;
    private float cellSize;
    
    private Vector3 originPosition;
    private int height;
    private int width;
    
    private GameObject parent;
    private bool showDebug = false;
    private TextMeshPro[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[this.width, this.height];

        for (var y = 0; y < gridArray.GetLength(1); y++)
        {
            for (var x = 0; x < gridArray.GetLength(0); x++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

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


                    debugTextArray[x, y] = UtilityLibrary.CreateWorldText(gridArray[x, y]?.ToString(), parent.transform,
                        GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 4, Color.white,
                        VerticalAlignmentOptions.Middle, HorizontalAlignmentOptions.Center);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (sender, args) =>
            {
                debugTextArray[args.x, args.y].text = gridArray[args.x, args.y]?.ToString();
            };
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

    public void SetGridObject(int x, int y, TGridObject gridObject)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = gridObject;
            if (showDebug) 
                debugTextArray[x, y].text = gridObject.ToString();
            
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{x = x, y = y});
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject gridObject)
    {
        int x, y;

        GetGridPosition(worldPosition, out x, out y);

        SetGridObject(x, y, gridObject);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{x = x, y = y});
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) return gridArray[x, y];

        return default;
    }

    public TGridObject GetGridObject(Vector3 wordPosition)
    {
        int x, y;
        GetGridPosition(wordPosition, out x, out y);

        return GetGridObject(x, y);
    }
}