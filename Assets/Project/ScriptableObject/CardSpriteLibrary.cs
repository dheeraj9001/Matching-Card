using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MemoryMatch/Card Sprite Library")]
public class CardSpriteLibrary : ScriptableObject
{
    public List<Sprite> sprites;

    public Sprite GetSprite(int id)
    {
        if (id < 0 || id >= sprites.Count)
            return null;

        return sprites[id];
    }

    public List<Sprite> GetShuffledSprites(int count)
    {
        var copy = new List<Sprite>(sprites);

        for (int i = 0; i < copy.Count; i++)
        {
            int rand = Random.Range(i, copy.Count);
            (copy[i], copy[rand]) = (copy[rand], copy[i]);
        }

        return copy.GetRange(0, count);
    }
}