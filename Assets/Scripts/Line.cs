using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface
public enum Spawnables : short {None, Egg, Gem, Spike};
    
public class Line
{
    public Spawnables left;
    public Spawnables center;
    public Spawnables right;


    public Line(Spawnables l, Spawnables c, Spawnables r)
    {
        left = l;
        center = c;
        right = r;
    }
}