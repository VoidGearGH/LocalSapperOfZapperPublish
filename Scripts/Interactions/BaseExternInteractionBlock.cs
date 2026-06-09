using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseExternInteractionBlock
{
    public abstract void Animate(Vector3Int cellPos, TileBase pressed, TileBase normal, Tilemap tilemap);
    public abstract void Sound(AudioSource source, AudioClip clip);
}
