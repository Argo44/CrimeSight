using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Menu,
    Game
}

public class GameManager : MonoBehaviour
{

    // Fields
    static private GameState state;

    public delegate void SightEventHandler();
    static public event SightEventHandler OnSight;

    // Properties
    static public GameState State
    {
        get { return state; }
        set { state = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Game;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ActivateSight()
    {
        OnSight?.Invoke();
    }
}
