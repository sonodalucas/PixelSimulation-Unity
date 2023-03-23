using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGrid : MonoBehaviour
{
    private Grid<bool> grid;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<bool>(10, 10, 1f, Vector3.zero, 
            (Grid<bool> g, int x, int y) => false);
        
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
