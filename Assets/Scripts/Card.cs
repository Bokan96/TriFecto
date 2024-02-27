using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Card
{
    public int Id { get; }
    public int Power { get; }
    public int Faction { get; }
    public bool IsFlipped { get; }

    public Card(int id, int power, int faction, bool isFlipped)
    {
        Id = id;
        Power = power;
        Faction = faction;
        IsFlipped = isFlipped;
    }

    
    public Card(int id)
    {
        Id = id;
        Power = (id - 1) % 6 + 1;
        Faction = (id - 1) / 6 + 1;
        IsFlipped |= false;
    }


    public void DisplayCardInfo()
    {
        string cardName;

        switch (Id)
        {
            case 1:
                cardName = "Hinata";
                break;
            case 2:
                cardName = "Sakura";
                break;
            case 3:
                cardName = "Rock Lee";
                break;
            case 4:
                cardName = "Kakashi";
                break;
            case 5:
                cardName = "Sasuke";
                break;
            case 6:
                cardName = "Naruto";
                break;
            case 7:
                cardName = "Historia";
                break;
            case 8:
                cardName = "Erwin Smith";
                break;
            case 9:
                cardName = "Armin";
                break;
            case 10:
                cardName = "Eren Yeager";
                break;
            case 11:
                cardName = "Levi";
                break;
            case 12:
                cardName = "Mikasa";
                break;
            case 13:
                cardName = "May Cheng";
                break;
            case 14:
                cardName = "Armstrong";
                break;
            case 15:
                cardName = "Scar";
                break;
            case 16:
                cardName = "Roy Mustang";
                break;
            case 17:
                cardName = "Alphonse Elric";
                break;
            case 18:
                cardName = "Edvard Elric";
                break;
            default:
                cardName = "Bezimena";
                break;
        }

        Debug.Log($"{cardName}:  POW {Power}| FAC {Faction}");
    }

}
