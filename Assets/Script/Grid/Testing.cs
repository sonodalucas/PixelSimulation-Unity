using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private HeatMapVisual _heatMapVisual;
    private Grid<HeatMapGridObject> _grid;
    private Camera _mainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        _mainCamera = Camera.main;
       
        // Get screen size in world units
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;
        
        _grid = new Grid<HeatMapGridObject>(36, 20, 1f, 
            new Vector3(-(width / 2), -(height / 2), 0), 
            (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g,x,y));
        
        _heatMapVisual.SetGrid(_grid);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = UtilityLibrary.GetMouseWorldPosition(_mainCamera);
            HeatMapGridObject heatMapGridObject = _grid.GetGridObject(mousePosition);
            heatMapGridObject?.AddValue(5);
        }
    }
}

public class HeatMapGridObject
{
    private const int MAX = 100;
    private const int MIN = 0;

    private Grid<HeatMapGridObject> grid;
    private int value;
    private int x;
    private int y;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public void AddValue(int newValue)
    {
        value += newValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}