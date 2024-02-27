using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int Id { get; private set; }
    public int Power { get; private set; }
    public int Faction { get; private set; }

    public Card(int id, int power, int faction)
    {
        Id = id;
        Power = power;
        Faction = faction;
    }

    public void DisplayCardInfo()
    {
        Debug.Log($"Card ID: {Id}, Power: {Power}, Faction: {Faction}");
    }

}
