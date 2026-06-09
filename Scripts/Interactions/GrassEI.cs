using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
public class GrassEI : BaseExternInteractionBlock
{
    public override void Animate(Vector3Int cellPos, TileBase pressed, TileBase normal, Tilemap tilemap)
    {
        CoroutineRunner.Instance.RunCoroutine(StopForAnim(cellPos, pressed, normal, tilemap));
    }
    public override void Sound(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip, 0.4f);
    }
    private IEnumerator StopForAnim(Vector3Int cellPos, TileBase pressed, TileBase normal, Tilemap tilemap)
    {
        tilemap.SetTile(cellPos, pressed);
        tilemap.RefreshTile(cellPos);

        yield return new WaitForSeconds(1f);

        tilemap.SetTile(cellPos, normal);
        tilemap.RefreshTile(cellPos);
    }
}
