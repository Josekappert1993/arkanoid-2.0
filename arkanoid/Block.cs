using System;

public enum Color { Black = 0, Yellow, Blue, Green, Purple };
public class Block
{
    private int hitCount;
    private int maxHits;
    Color _color;
	public Block(int maxHits, Color _color)
	{
        this.maxHits = maxHits;
        this._color = _color;
	}

    public void setMaxHits(int maxHits)
    {
       
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
