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

    private GridLayoutGroup grid;
    private RectTransform panelRect;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        panelRect = transform.parent as RectTransform;
        UpdateCellSize();
    }

    void OnEnable()
    {
        if (grid == null) grid = GetComponent<GridLayoutGroup>();
        if (panelRect == null)
            panelRect = transform.parent as RectTransform;
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
        if (panelRect == null || grid == null || maxColumns <= 0 || maxRows <= 0) return;

        float totalWidth = panelRect.rect.width;
        float totalHeight = panelRect.rect.height;
        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;
        float paddingX = grid.padding.left + grid.padding.right;
        float paddingY = grid.padding.top + grid.padding.bottom;

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
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        cellWidth = (totalWidth - paddingX - spacingX * (columns - 1)) / columns;
        cellHeight = (totalHeight - paddingY - spacingY * (rows - 1)) / rows;

        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
