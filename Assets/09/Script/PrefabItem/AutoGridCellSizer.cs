using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGridCellSizer : MonoBehaviour
{
    public int maxColumns = 6;
    public int maxRows = 2;
    public float minCellWidth = 80f;
    public float minCellHeight = 80f;

    private GridLayoutGroup _grid;
    private RectTransform _panelRect;

    void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _panelRect = transform.parent as RectTransform;
        UpdateCellSize();
    }

    void OnEnable()
    {
        if (_grid == null) _grid = GetComponent<GridLayoutGroup>();
        if (_panelRect == null)
            _panelRect = transform.parent as RectTransform;
        UpdateCellSize();
    }

    void Update()
    {
#if UNITY_EDITOR
        UpdateCellSize();
#endif
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateCellSize();
    }

    public void UpdateCellSize()
    {
        if (_panelRect == null || _grid == null || maxColumns <= 0 || maxRows <= 0) return;

        float totalWidth = _panelRect.rect.width;
        float totalHeight = _panelRect.rect.height;
        float spacingX = _grid.spacing.x;
        float spacingY = _grid.spacing.y;
        float paddingX = _grid.padding.left + _grid.padding.right;
        float paddingY = _grid.padding.top + _grid.padding.bottom;

        int columns = maxColumns;
        int rows = maxRows;
        float cellWidth, cellHeight;

        // Lặp để giảm cột/hàng cho đến khi cell không bị nhỏ hơn min
        while (columns > 1)
        {
            cellWidth = (totalWidth - paddingX - spacingX * (columns - 1)) / columns;
            if (cellWidth >= minCellWidth)
                break;
            columns--;
        }
        while (rows > 1)
        {
            cellHeight = (totalHeight - paddingY - spacingY * (rows - 1)) / rows;
            if (cellHeight >= minCellHeight)
                break;
            rows--;
        }

        // Gán lại số cột (chỉ nên dùng Fixed Column Count hoặc Fixed Row Count)
        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = columns;

        cellWidth = (totalWidth - paddingX - spacingX * (columns - 1)) / columns;
        cellHeight = (totalHeight - paddingY - spacingY * (rows - 1)) / rows;

        _grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
