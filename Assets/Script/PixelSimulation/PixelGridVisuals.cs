using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGridVisuals : MonoBehaviour
{
    private Grid<Cell> _grid;
    private Mesh _mesh;
    private bool _updateMesh;
    // Start is called before the first frame update
    void Awake()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void LateUpdate()
    {
        if (_updateMesh)
        {
            _updateMesh = false;
            UpdatePixelGridVisual();
        }
    }

    public void SetGrid(Grid<Cell> grid)
    {
        _grid = grid;
        UpdatePixelGridVisual();
    
        _grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<Cell>.OnGridValueChangedEventArgs e)
    {
        // UpdateHeatMapVisual();
        _updateMesh = true;
    }

    private void UpdatePixelGridVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(_grid.GetWidth() * _grid.GetHeight(), out Vector3[] vertices, 
            out Vector2[] uv, out int[] triangles);
        
        for (int y = 0; y < _grid.GetHeight(); y++)
        {
            for (int x = 0; x < _grid.GetWidth(); x++)
            {
                
                // int index = y * _grid.GetHeight() + x;
                int index = y * _grid.GetWidth() + x;
    
                Vector3 quadSize = new Vector3(1, 1) * _grid.GetCellSize();
                Vector3 quadPosition = _grid.GetWorldPosition(x, y) + quadSize * .5f;
    
                Cell gridValue = _grid.GetGridObject(x, y);
                // float gridValueNormalized = gridValue.value > 0 ? 1f : 0f;
                Vector2 gridValueUV = new Vector2(gridValue.GetPixelTypeValueNormalized(), 0);
    
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, quadPosition, 0f, quadSize, 
                    gridValueUV, gridValueUV);
            }
        }
    
        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
    }
}
