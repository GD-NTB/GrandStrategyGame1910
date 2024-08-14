using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditorCore : MonoBehaviour
{
    void Start()
    {
        MapParent.mapLoader.CreateEverything();

        // set current view to states mode
        MapParent.mapState.ChangeCurrentMapView(MapState.MapView.States);
    }

    private Vector2Int mouseCoords;

    void Update()
    {
        mouseCoords = MapParent.mapUtils.GetCoordsAtMouse();
        
        // selecting states only works in views other than tiles view
        if (MapParent.mapState.CurrentMapView != MapState.MapView.Tiles)
        {
            // select hovered over state on mouse 0
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // prevent click through ui
            {
                SelectState(MapParent.mapUtils.GetStateAtCoords(mouseCoords));
            }
        }

        // todo: remove
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            MapParent.mapLoader.CreateEverything();
            MapParent.mapUtils.RedrawMap();
        }
    }

    public State SelectedState;
    public void SelectState(State newState)
    {
        // 1 - recolour old state
        if (SelectedState != null)
        {
            MapParent.mapUtils.RedrawState(SelectedState);
            print($"deselected state {SelectedState.ID}");
        }
        
        // 2 - select new state
        if (newState == null) // clicked on no state, deselect and fuck off
        {
            SelectedState = null;
            return;
        }
        if (newState == SelectedState) // clicked on same state, deselect and piss off
        {
            SelectedState = null;
            return;
        } 

        SelectedState = newState;
        
        // 3 - colour new state
        // MapParent.mapUtils.SetStateColour(newState, Color.red);
        MapParent.mapUtils.OutlineState(newState, Color.red, 1.0f, 0.5f);

        print($"selected state {SelectedState.ID}");
    }
}
