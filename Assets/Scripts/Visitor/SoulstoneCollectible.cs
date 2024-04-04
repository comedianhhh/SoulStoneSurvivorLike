using UnityEngine;

public class SoulstoneCollectible : MonoBehaviour, ICollectible
{
    private SpriteRenderer spriteRenderer;
    public StonesDataSO soulstoneData;
    public int quantity;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = soulstoneData.GetSprite();
    }

    public void Accept(IPlayerVisitor visitor)
    {
        visitor.Visit(this);
        Destroy(gameObject, 0.1f);
    }
}
