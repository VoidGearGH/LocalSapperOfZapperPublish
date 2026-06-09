using UnityEngine;
using UnityEngine.Tilemaps;

public class DefaultEI : BaseExternInteractionBlock
{
    public override void Animate(Vector3Int cellPos, TileBase pressed, TileBase normal, Tilemap tilemap)
    {

    }
    public override void Sound(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip, 0.3f);
    }
}
