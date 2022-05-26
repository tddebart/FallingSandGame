

public abstract class Element
{
    public int x;
    public int y;
    public SandBox sandBox;

    protected Element(int x, int y, SandBox sandBox)
    {
        this.x = x;
        this.y = y;
        this.sandBox = sandBox;
    }

    public abstract void Step(bool[,] map, bool[,] mapCopy);


    public bool GetCell(int x, int y)
    {
        return sandBox.mapCopy[x, y];
    }
    
    public void MoveTo(int x, int y)
    {
        sandBox.map[this.x, this.y] = false;
        sandBox.map[x, y] = true;
        this.x = x;
        this.y = y;
    }
} 

