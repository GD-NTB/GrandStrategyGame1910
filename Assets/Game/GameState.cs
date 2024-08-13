using UnityEngine;

public class GameState : MonoBehaviour
{
    // ---------------------------------------------------- GAME STAGES ----------------------------------------------------
    [ReadOnly] public bool IsChoosingCountry;

    // ---------------------------------------------------- COUNTRY ----------------------------------------------------
    public Country PlayerCountry;
}
