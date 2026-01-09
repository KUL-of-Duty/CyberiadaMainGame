using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscSampling : MonoBehaviour
{
    [Header("Poisson Base Values")]
    [SerializeField] float radius = 3;
    [SerializeField] int seed = 10000;
    [SerializeField] int width = 512;
    [SerializeField] int height = 512;

    [Header("Grid system")]
    float gridSize;
    int[,] grid;

    [Header("Lists")]
    List<Vector2> activePoints = new List<Vector2>();
    public List<Vector2> points = new List<Vector2>();

    System.Random rng;

    public void Start()
    {
        rng = new System.Random(seed);
        gridSize = radius / Mathf.Sqrt(2f);
        grid = new int[Mathf.CeilToInt(width / gridSize), Mathf.CeilToInt(height / gridSize)];

        // Initialize grid to -1
        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
                grid[x, y] = -1;

        // Add first point
        Vector2 firstPoint = new Vector2((float)(width * rng.NextDouble()), (float)(height * rng.NextDouble()));
        activePoints.Add(firstPoint);
        points.Add(firstPoint);
        FillGridAt(firstPoint, 0);
        GeneratePoints();
    }

    void GeneratePoints()
    {
        const int k = 30;

        while (activePoints.Count > 0)
        {
            int index = (int)(rng.NextDouble() * activePoints.Count);
            Vector2 oldPoint = activePoints[index];
            bool found = false;

            for (int i = 0; i < k; i++)
            {
                float angle = (float)rng.NextDouble() * Mathf.PI * 2f;
                float distance = radius + (float)rng.NextDouble() * radius;
                Vector2 candidate = oldPoint + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

                if (IsValid(candidate))
                {
                    points.Add(candidate);
                    activePoints.Add(candidate);
                    FillGridAt(candidate, points.Count - 1);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                activePoints.RemoveAt(index);
            }
        }
    }

    void FillGridAt(Vector2 point, int pointIndex)
    {
        int cellX = Mathf.Clamp((int)(point.x / gridSize), 0, grid.GetLength(0) - 1);
        int cellY = Mathf.Clamp((int)(point.y / gridSize), 0, grid.GetLength(1) - 1);
        grid[cellX, cellY] = pointIndex;
    }

    bool IsValid(Vector2 candidate)
    {
        // Check bounds
        if (candidate.x < 0 || candidate.x >= width || candidate.y < 0 || candidate.y >= height)
            return false;

        int cellX = (int)(candidate.x / gridSize);
        int cellY = (int)(candidate.y / gridSize);

        int startX = Mathf.Max(0, cellX - 2);
        int endX = Mathf.Min(grid.GetLength(0) - 1, cellX + 2);
        int startY = Mathf.Max(0, cellY - 2);
        int endY = Mathf.Min(grid.GetLength(1) - 1, cellY + 2);

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                int index = grid[x, y];
                if (index != -1)
                {
                    Vector2 other = points[index]; // use stable points list
                    if ((candidate - other).sqrMagnitude < radius * radius)
                        return false;
                }
            }
        }

        return true;
    }
}
