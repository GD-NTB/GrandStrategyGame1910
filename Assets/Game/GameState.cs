using UnityEngine;

public class GameState : MonoBehaviour
{
    // ---------------------------------------------------- GAME STAGES ----------------------------------------------------
    [ReadOnly] public bool IsChoosingCountry;

    // ---------------------------------------------------- COUNTRY ----------------------------------------------------
    public Country PlayerCountry;

    // ---------------------------------------------------- MAP CONTROL ----------------------------------------------------
    public GameTile SelectedTile;
    public State SelectedState;
    public Country SelectedCountry;
}
