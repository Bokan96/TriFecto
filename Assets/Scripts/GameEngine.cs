using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class GameEngine : MonoBehaviour
{
    public Player[] players;
    public Deck deck;
    public Field field;

    void Start()
    {
        players = new Player[2];
        players[0] = new Player(0, 10);
        players[1] = new Player(1, 10);
        deck = new Deck();
        deck.Shuffle();
        field = new Field();

        for(int i = 6; i > 0; i--)
        {
            deck.DealCardToPlayer(players[0]);
            deck.DealCardToPlayer(players[1]);
        }
        players[0].SortCardsInHandById();
        players[1].SortCardsInHandById();

        DisplayGameState();
    }
    public void DisplayGameState()
    {
        Debug.Log("Current Game State:");
        players[0].DisplayPlayerInfo();
        players[1].DisplayPlayerInfo();
        field.DisplayFieldState();
    }
}