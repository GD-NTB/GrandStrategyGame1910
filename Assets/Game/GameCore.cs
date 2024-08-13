using UnityEngine;

public class GameCore : MonoBehaviour
{
    void Start()
    {
        MapParent.mapLoader.CreateEverything();

        // set current view to states mode
        MapParent.mapState.ChangeCurrentMapView(MapState.MapView.Countries);

        // start choosing country stuff
        GameParent.gameChooseCountry.StartChoosingCountry();
    }

    public Vector2Int mouseCoords { get; private set; }
    public bool IsMouseOnLand { get; private set; }

    void Update()
    {
        mouseCoords = MapParent.mapUtils.GetCoordsAtMouse();
        IsMouseOnLand = MapParent.mapUtils.AreCoordsOnLand(mouseCoords);
    }
}
