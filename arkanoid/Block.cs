using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum Color { Black, Yellow, Blue, Green, Purple };
public class Block
{
    protected string textureName = "";
    protected Texture2D texture;
    protected Game game;
    private int hitCount;
    private int maxHits;
    Color _color;

    public Block(Color color)
	{
        setMaxHits(color);
    }

    public void setMaxHits(Color color)
    {
        color = setColor(color);
        switch (color) {
            case Black:
                maxHits = 1;
                break;
            case Yellow:
                maxHits = 8;
                break;
            case Blue:
                maxHits = 3;
                break;
            case Green:
                maxHits = 2;
                break;
            case Purple:
                maxHits = 6;
                break;
            
        }
       
    }

    public int getMaxHits()
    {
        return maxHits;
    }

    public void setColor(Color blockColor)
    {
        _color = blockColor;
    }

    public Color getColor()
    {
        return _color;
    }

    public void addHit()
    {
        hitCount += 1;
    }

    public string getUpgrade()
    {
        return null;
    }
}
