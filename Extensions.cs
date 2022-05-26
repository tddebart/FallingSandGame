
public static class Extensions
{
    public static Vector4 ToVector4(this Color color)
    {
        return new Vector4(color.R/255, color.G/255, color.B/255, color.A/255);
    }
    
    public static Color ToColor(this Vector4 vector)
    {
        return new Color((byte)(vector.X*255f), (byte)(vector.Y*255f), (byte)(vector.Z*255f), (byte)(vector.W*255f));
    }
}
