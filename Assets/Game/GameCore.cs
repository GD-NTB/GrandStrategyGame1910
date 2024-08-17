using UnityEngine;
using UnityEngine.EventSystems;

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

        // if we're out of choosing country screen
        if (!GameParent.gameState.IsChoosingCountry)
        {
            DoSelectingState();
        }
    }
    
    // ---------------------------------------------------- MAP CONTROL ----------------------------------------------------
    private void DoSelectingState()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // prevent click through ui
        {
            DoSelectState(MapParent.mapUtils.GetStateAtCoords(mouseCoords));
        }
    }

    private void DoSelectState(State newState)
    {
        //  if clicked on no state, deselect, recolour old state, and fuck off
        if (newState == null)
        {
            DeselectState();
            return;
        }

        SelectState(newState);
    }

    public void SelectState(State newState)
    {
        if (newState == GameParent.gameState.SelectedState) { return; } // if selecting same state, do nothing
        
        // 1 - redraw old state
        if (GameParent.gameState.SelectedState != null)
        {
            MapParent.mapUtils.RedrawState(GameParent.gameState.SelectedState, false);
        }
        
        // 2 - select new state
        GameParent.gameState.SelectedState = newState;
        GameParent.gameState.SelectedTile = MapParent.mapUtils.GetTileAtCoords(mouseCoords, true); // we already did land check
        GameParent.gameState.SelectedCountry = MapParent.mapUtils.GetCountryByTag(GameParent.gameState.SelectedTile.OccupiedByCountryTag);
        
        // 3 - draw new state
        // MapParent.mapUtils.SetStateColour(newState, Color.red);
        MapParent.mapUtils.OutlineState(newState, Color.red, 1.0f, 0.5f); // todo: use mesh outline instead

        // open state info ui panel
        UIPanel stateInfoPanel = UIPanel.FindByName("StateInfoPanel");
        if (!stateInfoPanel.IsOpen)
        {
            stateInfoPanel.OnOpen();
        }

        print($"selected state {GameParent.gameState.SelectedState.ID}");
    }

    public void DeselectState()
    {
        // if nothing is selected, get out mate
        if (GameParent.gameState.SelectedState == null) { return; }

        // redraw selected state
        MapParent.mapUtils.RedrawState(GameParent.gameState.SelectedState, false);

        print($"deselected state {GameParent.gameState.SelectedState.ID}");
        GameParent.gameState.SelectedState = null;
        
        // close state info ui panel
        UIPanel stateInfoPanel = UIPanel.FindByName("StateInfoPanel");
        if (stateInfoPanel.IsOpen)
        {
            stateInfoPanel.OnClose();
        }
        
        return;
    }
}
