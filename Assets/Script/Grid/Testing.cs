using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private HeatMapVisual _heatMapVisual;
    private Grid _grid;
    private Camera _mainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        _mainCamera = Camera.main;
       
        // Get screen size in world units
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;
        
        _grid = new Grid(10, 10, 1f, new Vector3(-(width / 2), -(height / 2), 0));
        _heatMapVisual.SetGrid(_grid);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = UtilityLibrary.GetMouseWorldPosition(_mainCamera);
            // int value = _grid.GetValue(mousePosition);
            // _grid.SetValue(mousePosition, value + 5);
            _grid.AddValue(mousePosition, 100, 10, 1);
        }
           
    }
}