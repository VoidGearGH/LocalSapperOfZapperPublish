using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BoundEI : BaseExternInteractionBlock
{
    public override void Animate(Vector3Int cellPos, TileBase pressed, TileBase normal, Tilemap tilemap)
    {
        CoroutineRunner.Instance.RunCoroutine(StopForAnimation(cellPos, pressed, normal, tilemap));
    }
    public override void Sound(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip, 0.5f);
    }

    private IEnumerator StopForAnimation(Vector3Int cellPos, TileBase pressed, TileBase normal, Tilemap tilemap)
    {
        tilemap.SetTile(cellPos, pressed);
        tilemap.RefreshTile(cellPos);

        yield return new WaitForSeconds(0.7f);

        tilemap.SetTile(cellPos, normal);
        tilemap.RefreshTile(cellPos);
    }
}
