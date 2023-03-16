using System;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableHero : ScriptableObject
{
    public HeroType Type;
    public Sprite Image;
    public Color Color;
}

[Serializable]
public enum HeroType
{
    Dino = 0,
    Snorlax = 1,
    Jolteon = 2,
}