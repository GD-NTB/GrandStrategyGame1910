using UnityEngine;

public class GameParent : MonoBehaviour
{
    public static GameCore gameCore { get; private set; }
    public static GameState gameState { get; private set; }
    public static GameChooseCountry gameChooseCountry { get; private set; }

    void Awake()
    {
        gameCore = transform.Find("GameCore").GetComponent<GameCore>();
        gameState = transform.Find("GameState").GetComponent<GameState>();
        gameChooseCountry = transform.Find("GameChooseCountry").GetComponent<GameChooseCountry>();
    }
}
