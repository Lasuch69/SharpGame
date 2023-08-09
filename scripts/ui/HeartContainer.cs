using System.Collections.Generic;

namespace SharpGame;

public partial class HeartContainer : GridContainer
{
    [Export]
    public Texture2D HeartTexture;

    [Export]
    public Material HeartMaterial;

    List<TextureRect> _textureRectsPool = new(10);

    public void SetHearts(int count)
    {
        while (count > _textureRectsPool.Count)
        {
            var textureRect = CreateTextureRect();
            AddChild(textureRect);
            _textureRectsPool.Add(textureRect);
        }

        for (int i = 0; i < _textureRectsPool.Count; i++)
        {
            _textureRectsPool[i].Visible = i < count;
        }
    }

    TextureRect CreateTextureRect() => new()
    {
        StretchMode = TextureRect.StretchModeEnum.KeepAspect,
        Texture = HeartTexture,
        Material = HeartMaterial
    };
}
