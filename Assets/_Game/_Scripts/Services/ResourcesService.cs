using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ResourcesService
{
    public static List<ScriptableHero> Heroes { get; }

    static ResourcesService()
    {
        Heroes = Resources.LoadAll<ScriptableHero>("Heroes").OrderBy(h => (int)h.Type).ToList();
    }

    public static ScriptableHero GetHeroByType(HeroType type)
    {
        return Heroes.First(h => h.Type == type);
    }
}