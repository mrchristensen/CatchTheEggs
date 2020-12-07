using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface
public enum Spawnables : short {None, Egg, Gem, Spike};
    //todo: make a spawnQueue object that holds lines and has all the logic for generating a spawwn queue in that object
public class Line
{
    public Spawnables left;
    public Spawnables center;
    public Spawnables right;


    public Line(Spawnables l, Spawnables c, Spawnables r) //Constructor
    {
        left = l;
        center = c;
        right = r;
    }
}