
public class Liquid : Element
{
    public Liquid(int x, int y, SandBox sandBox) : base(x, y, sandBox)
    {
    }

    public override void Step(bool[,] map, bool[,] mapCopy)
    {
        var targetCell = GetCell(x, y - 1);
    }
}
