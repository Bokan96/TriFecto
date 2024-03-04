using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Card
{
    public int Id { get; set; }
    public int Power { get; set; }
    public int Faction { get; set; }
    public bool IsFlipped { get; set; }
    public String Name { get; set; }

    public Card(int id, int power, int faction, bool isFlipped, String name)
    {
        Id = id;
        Power = power;
        Faction = faction;
        IsFlipped = isFlipped;
        Name = name;
    }

    
    public Card(int id)
    {
        Id = id;
        Power = (id - 1) % 6 + 1;
        Faction = (id - 1) / 6;
        IsFlipped |= false;
        Name = id switch
        {
            1 => "Hinata",
            2 => "Sakura",
            3 => "Rock Lee",
            4 => "Kakashi",
            5 => "Sasuke",
            6 => "Naruto",
            7 => "Historia",
            8 => "Erwin Smith",
            9 => "Armin",
            10 => "Eren Yeager",
            11 => "Levi",
            12 => "Mikasa",
            13 => "May Cheng",
            14 => "Armstrong",
            15 => "Scar",
            16 => "Roy Mustang",
            17 => "Alphonse Elric",
            18 => "Edvard Elric",
            _ => "Bezimena",
        };
    }


    public void DisplayCardInfo()
    {
        Debug.Log($"{Name}:  POW {Power}| FAC {Faction}");
    }

    public void Flip()
    {
        IsFlipped = !IsFlipped;
    }

    public override string ToString()
    {
        char faction;
        if (Faction == 0)
            faction = 'C';
        else if (Faction == 1)
            faction = 'G';
        else if (Faction == 2)
            faction = 'B';
        else
            faction = 'X';
        return $"[{faction}-{Power}] {Name}";
    }
}
