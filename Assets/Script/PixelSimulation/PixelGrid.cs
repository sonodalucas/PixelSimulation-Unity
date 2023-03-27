using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PixelGrid : MonoBehaviour
{
    [SerializeField] private PixelGridVisuals gridVisuals;
    private Grid<Cell> grid;
    private Camera mainCamera;
    private CellType _cellTypeToInsert = CellType.Sand;
    private bool moveCells = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        
        // Get screen size in world units
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;
     
        grid = new Grid<Cell>(35, 20, 1f,new Vector3(-(width / 2), -(height / 2), 0), 
            (Grid<Cell> g, int x, int y) => new Cell(g, x, y));
        
        gridVisuals.SetGrid(grid);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = UtilityLibrary.GetMouseWorldPosition(mainCamera);
            Cell cellObject = grid.GetGridObject(mousePosition);

            cellObject?.SetCellType(_cellTypeToInsert);
            moveCells = true;
        }
    }

    private void FixedUpdate()
    {
        if (!moveCells) return;
        bool gridChanged = false;
        for (int y = 0; y < grid.GetHeight(); y++)
        {
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                Cell cellObject = grid.GetGridObject(x, y);
                if (TryToMoveCell(cellObject.GetPossibleMovements(), x, y, cellObject.GetCellType()))
                {
                    gridChanged = true;
                }
            }
        }
        
        if (!gridChanged)
        {
            moveCells = false;
        }
    }

    // TODO: Move cells based on speed
    private bool TryToMoveCell(CellMovement[] possibleMoves, int x, int y, CellType cellType)
    {
        if (possibleMoves.Contains(CellMovement.None)) return false;

        foreach (var move in possibleMoves)
        {
            Cell[] possibleFinalCells;
            switch (move)
            {
                case CellMovement.None:
                    possibleFinalCells = Array.Empty<Cell>();
                    break;
                case CellMovement.Down:
                    possibleFinalCells = new Cell[1];
                    possibleFinalCells[0] = grid.GetGridObject(x, y - 1);
                    break;
                case CellMovement.DownSides:
                    possibleFinalCells = new Cell[2];
                    possibleFinalCells[0] = grid.GetGridObject(x - 1, y - 1);
                    possibleFinalCells[1] = grid.GetGridObject(x + 1, y - 1);
                    break;
                case CellMovement.Sides:
                    possibleFinalCells = new Cell[2];
                    possibleFinalCells[0] = grid.GetGridObject(x - 1, y);
                    possibleFinalCells[1] = grid.GetGridObject(x + 1, y);
                    break;
                case CellMovement.Up:
                    possibleFinalCells = new Cell[1];
                    possibleFinalCells[0] = grid.GetGridObject(x, y + 1);
                    break;
                case CellMovement.UpSides:
                    possibleFinalCells = new Cell[2];
                    possibleFinalCells[0] = grid.GetGridObject(x - 1, y + 1);
                    possibleFinalCells[1] = grid.GetGridObject(x + 1, y + 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var finalCell in possibleFinalCells)
            {
                if (finalCell == null) continue;
                if (finalCell.GetCellType() != CellType.Empty) continue;

                finalCell.SetCellType(cellType);
                grid.GetGridObject(x, y).SetCellType(CellType.Empty);
                return true;
            }
        }

        return false;
    }
}
