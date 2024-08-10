using UnityEngine;
using UnityEngine.SceneManagement;

public class MapParent : MonoBehaviour
{
    // Game
    public static MapState mapState { get; private set; }
    public static MapCreator mapCreator { get; private set; }
    public static MapUtils mapUtils { get; private set; }
    public static MapLoader mapLoader { get; private set; }

    // MapEditor
    public static MapEditorCore mapEditorCore { get; private set; }
    public static MapEditorUI mapEditorUI { get; private set; }

    void Awake()
    {
        mapState = transform.Find("MapState").GetComponent<MapState>();
        mapCreator = transform.Find("MapCreator").GetComponent<MapCreator>();
        mapUtils = transform.Find("MapUtils").GetComponent<MapUtils>();
        mapLoader = transform.Find("MapLoader").GetComponent<MapLoader>();

        // // if in map editor
        if (SceneManager.GetActiveScene().name == "MapEditor")
        {
            mapEditorCore = GameObject.Find("MapEditorCore").GetComponent<MapEditorCore>();
            mapEditorUI = GameObject.Find("MapEditorUI").GetComponent<MapEditorUI>();
        }
    }
}
