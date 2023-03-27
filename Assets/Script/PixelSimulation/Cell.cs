using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Empty = 0,
    Sand = 1,
    Water = 2,
    Stone = 3
}

public enum CellMovement
{
    None = 0,
    Down = 1,
    DownSides = 2,
    Sides = 3,
    Up = 4,
    UpSides = 5
}

public class Cell
{
    
    
    private Grid<Cell> grid;
    private int x;
    private int y;
    private CellType type;
    private int speed = 1;
    private CellMovement[] possibleMovements;

    public Cell(Grid<Cell> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;

        type = CellType.Empty;
        possibleMovements = new CellMovement[1];
        possibleMovements[0] = CellMovement.None;
    }

    public void SetCellType(CellType cellType)
    {
        this.type = cellType;
        SetCellMovementType();
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetPixelTypeValueNormalized()
    {
        return (float)type / 3;
    }

    public CellType GetCellType()
    {
        return type;
    }

    public CellMovement[] GetPossibleMovements()
    {
        return possibleMovements;
    }

    public int GetCellSpeed()
    {
        return speed;
    }

    private void SetCellMovementType()
    {
        switch (type)
        {
            case CellType.Empty:
                possibleMovements = new CellMovement[1];
                possibleMovements[0] = CellMovement.None;
                break;
            case CellType.Sand:
                possibleMovements = new CellMovement[2];
                possibleMovements[0] = CellMovement.Down;
                possibleMovements[1] = CellMovement.DownSides;
                break;
            case CellType.Water:
                possibleMovements = new CellMovement[3];
                possibleMovements[0] = CellMovement.Down;
                possibleMovements[1] = CellMovement.DownSides;
                possibleMovements[2] = CellMovement.Sides;
                break;
            case CellType.Stone:
                possibleMovements = new CellMovement[1];
                possibleMovements[0] = CellMovement.None;
                break;
        }
    }

    public override string ToString()
    {
        return type.ToString();
    }
}
