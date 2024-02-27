using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class GameEngine : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public Deck deck;
    public Field field;

    void Start()
    {
        player1 = new Player(0, 20);
        player2 = new Player(1, 20);
        deck = new Deck();
        deck.Shuffle();
        field = new Field();

        for(int i = 6; i > 0; i--)
        {
            deck.DealCardToPlayer(player1);
            deck.DealCardToPlayer(player2);
        }
        player1.SortCardsInHandById();
        player2.SortCardsInHandById();

        DisplayGameState();
    }
    public void DisplayGameState()
    {
        Debug.Log("Current Game State:");
        player1.DisplayPlayerInfo();
        player2.DisplayPlayerInfo();
        field.DisplayFieldState();
    }
}