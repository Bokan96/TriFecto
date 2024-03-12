using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class GameEngine : MonoBehaviour
{
    public static Player[] players;
    public static Deck deck;
    public static Field field;
    public static int turn;

    void Start()
    {
        players = new Player[2];

        //deck = new Deck();
        int[] testirane_karte = { 2, 3, 5, 7, 9, 15, 16, 17};
        deck = new Deck(testirane_karte);
        deck.Shuffle();
        field = new Field();
        turn = 0;

        for(int p = 0; p < players.Length; p++)
        {
            players[p] = new Player(p, 10);
            for (int i = 6; i > 0; i--)
                deck.DealCardToPlayer(players[p]);
            players[p].SortCardsInHandById();
        }

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