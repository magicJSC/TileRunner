using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class HexMapEditor : EditorWindow
{
    HexMapSO currentMap;
    TileType selectedType;

    [Header("Grid")]
    int radius = 5;
    float hexSize = 25f;

    Texture2D hexNormal;

    [MenuItem("Tools/Hex Map Editor")]
    static void Open()
    {
        GetWindow<HexMapEditor>("Hex Map Editor");
    }

    void OnEnable()
    {
        hexNormal = Resources.Load<Texture2D>("Hex");
    }

    void OnGUI()
    {
        DrawTopUI();

        if (currentMap == null)
            return;

        DrawHexGrid();

        GUILayout.Space(10);

        DrawBottomButtons();

    }

    // =========================
    // »ó´Ü UI
    // =========================
    void DrawTopUI()
    {
        EditorGUILayout.BeginHorizontal();

        currentMap = (HexMapSO)EditorGUILayout.ObjectField(
            "Map Data",
            currentMap,
            typeof(HexMapSO),
            false);

        if (GUILayout.Button("New", GUILayout.Width(60)))
            CreateNewMap();

        EditorGUILayout.EndHorizontal();

        selectedType = (TileType)GUILayout.Toolbar(
            (int)selectedType,
            System.Enum.GetNames(typeof(TileType)));
    }

    void CreateNewMap()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Create Hex Map",
            "NewHexMap",
            "asset",
            "Save Hex Map");

        if (string.IsNullOrEmpty(path))
            return;

        HexMapSO map = CreateInstance<HexMapSO>();
        map.tiles = new List<TileInfo>();

        AssetDatabase.CreateAsset(map, path);
        AssetDatabase.SaveAssets();

        currentMap = map;
        EditorUtility.SetDirty(map);
    }

    void DrawBottomButtons()
    {
        if (currentMap == null) return;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("All ¡æ Normal", GUILayout.Height(30)))
        {
            FillAllTiles(TileType.Normal);
        }

        if (GUILayout.Button("All ¡æ Empty", GUILayout.Height(30)))
        {
            FillAllTiles(TileType.Empty);
        }

        GUILayout.EndHorizontal();
    }

    void FillAllTiles(TileType type)
    {
        Undo.RecordObject(currentMap, "Fill All Tiles");

        for (int q = -radius + 1; q < radius; q++)
        {
            for (int r = -radius + 1; r < radius; r++)
            {
                int s = -q - r;
                if (Mathf.Abs(s) >= radius) continue;

                Vector2Int coord = new Vector2Int(q, r);
                SetTile(coord, type);
            }
        }

        EditorUtility.SetDirty(currentMap);
    }

    // =========================
    // Hex Grid Draw
    // =========================
    void DrawHexGrid()
    {
        Rect area = GUILayoutUtility.GetRect(
            10, 1000, 10, 1000);

        Vector2 center = area.center;
        Event e = Event.current;

        for (int q = -(radius - 1); q <= radius - 1; q++)
        {
            for (int r = -(radius - 1); r <= radius - 1; r++)
            {
                Vector2Int coord = new Vector2Int(q, r);
                if (!IsInsideHex(coord))
                    continue;

                Vector2 pos = AxialToGUI(coord);
                Rect rect = new Rect(
                    center.x + pos.x - hexSize,
                    center.y + pos.y - hexSize,
                    hexSize * 1.8f,
                    hexSize * 1.8f);

                DrawHexButton(rect, coord, e);
            }
        }
    }

    // =========================
    // Button Draw
    // =========================
    void DrawHexButton(Rect rect, Vector2Int coord, Event e)
    {
        Texture2D tex = hexNormal;

        TileType tileType = GetTileType(coord);
        Color prevColor = GUI.color;

        GUI.color = GetColorByType(tileType);
        GUI.DrawTexture(rect, tex);
        GUI.color = prevColor;

        if (e.type == EventType.MouseDown &&
            rect.Contains(e.mousePosition))
        {
            SetTile(coord, selectedType);
            e.Use();
        }
    }

    TileType GetTileType(Vector2Int coord)
    {
        var tile = currentMap.tiles.Find(t => t.coord == coord);
        return tile != null ? tile.type : TileType.Empty;
    }

    // =========================
    // Helpers
    // =========================
    Vector2 AxialToGUI(Vector2Int coord)
    {
        float x = hexSize * Mathf.Sqrt(3f) * (coord.x + coord.y * 0.5f);
        float y = hexSize * 1.5f * coord.y;
        return new Vector2(x, y);
    }

    bool IsInsideHex(Vector2Int c)
    {
        int q = c.x;
        int r = c.y;
        int s = -q - r;

        return Mathf.Abs(q) < radius &&
               Mathf.Abs(r) < radius &&
               Mathf.Abs(s) < radius;
    }

 
    void SetTile(Vector2Int coord, TileType type)
    {
        var tile = currentMap.tiles.Find(t => t.coord == coord);

        if (TileType.Empty != type)
        {
            if (tile != null)
                tile.type = type;
            else
                currentMap.tiles.Add(new TileInfo
                {
                    coord = coord,
                    type = type
                });
        }
        else
        {
            if (tile != null)
                currentMap.tiles.Remove(tile);
        }

        EditorUtility.SetDirty(currentMap);
    }

    Color GetColorByType(TileType type)
    {
        switch (type)
        {
            case TileType.Start:
                return new Color(1, 0.7f, 0);
            case TileType.Normal:
                return Color.white;
            case TileType.Goal:
                return Color.green;
            case TileType.Obstacle:
                return Color.red;
            case TileType.Empty:
                return new Color(1,1,1, 0.2f);
            default:
                return Color.white;
        }
    }
}
