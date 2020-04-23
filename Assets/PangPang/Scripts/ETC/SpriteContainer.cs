using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteContainer
{
    static Dictionary<Category, Dictionary<string, Sprite>> spriteDic = new Dictionary<Category, Dictionary<string, Sprite>>();
    
    public enum Category
    {
        Ingame,
        Harppon
    }

    static string[] atlasPaths =
    {
        "Atlas/Ingame", "Atlas/Harpoon"
    };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init ()
    {
        for (int i = 0; i < atlasPaths.Length; i++)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(atlasPaths[i]);
            Category category = (Category)i;
            spriteDic.Add(category, new Dictionary<string, Sprite>());

            for (int j = 0; j < sprites.Length; j++)
            {
                spriteDic[category].Add(sprites[j].name, sprites[j]);
            }
        }
    }

    public static Sprite GetSprite (Category category, string name)
    {
        return spriteDic[category][name];
    }
}
