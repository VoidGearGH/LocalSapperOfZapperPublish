using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OnBlockClick : MonoBehaviour
{
    private void OnEnable()
    {
        FillingProcessor.EffectsOnClick += Sound;
        FillingProcessor.AnimateOnClick += Animate;
    }
    private void OnDisable()
    {
        FillingProcessor.EffectsOnClick -= Sound;
        FillingProcessor.AnimateOnClick -= Animate;
    }
    private void Animate(Vector3Int cellPos, TileBase pressed, TileBase normal, Tilemap tilemap, BaseExternInteractionBlock block)
    {
        block.Animate(cellPos, pressed, normal, tilemap);
    }
    private void Sound(AudioSource source, AudioClip clip, BaseExternInteractionBlock block)
    {
        block.Sound(source, clip);
    }
}
