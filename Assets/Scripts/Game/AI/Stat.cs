using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public int price { get; set; }
    public int ecology { get; set; }
    public int population { get; set; }
    public int energy { get; set; }
    public int pollution { get; set; }

    public void Reset()
    {
        price = 0;
        ecology = 0;
        population = 0;
        energy = 0;
        pollution = 0;
    }
    
    public void Add(Stat stat)
    {
        price += stat.price;
        ecology += stat.ecology;
        population += stat.population;
        energy += stat.energy;
        pollution += stat.pollution;
    }
    
    public void Subtract(Stat stat)
    {
        price -= stat.price;
        ecology -= stat.ecology;
        population -= stat.population;
        energy -= stat.energy;
        pollution -= stat.pollution;
    }
}
