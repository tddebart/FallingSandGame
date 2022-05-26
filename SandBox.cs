
using System.Diagnostics;

public class SandBox
{
    public const int width = 500;
    public const int height = 500;

    public static Vector4 backgroundColor = new(0.392f, 0.392f, 0.392f, 1.000f);
    public static Vector4 pixelColor = new(0.838f, 0.599f, 0.099f, 1.000f);
    
    public bool[,] map = new bool[width,height];
    public bool[,] mapCopy = new bool[width,height];
    public Color[,] colorMap = new Color[width,height];
    private readonly RenderWindow window;
    
    private Image image;
    private Texture texture;
    private Sprite sprite;

    public const int PixelSize = 2;
    public static int DrawRadius = 15;

    private static readonly Stopwatch updateClock = new();
    private static readonly Stopwatch drawClock = new();
    private bool diagnoseTime = false;

    public SandBox(RenderWindow window)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = false;
                mapCopy[x, y] = false;
                SetColor(x, y);
            }
        }
        
        this.window = window;
        
        image = new Image(colorMap);
        texture = new Texture(image);
        sprite = new Sprite(texture);
        sprite.Scale = new Vector2f(PixelSize, PixelSize);
    }
    
    public void Update()
    {
        if (diagnoseTime)
        {
            updateClock.Start();
        }
        
        // Check if we clicked on the window
        if ((Mouse.IsButtonPressed(Mouse.Button.Left) || Mouse.IsButtonPressed(Mouse.Button.Right)) && !ImGui.GetIO().WantCaptureMouse)
        {
            // Get the position of the mouse
            Vector2i mousePos = Mouse.GetPosition(window);

            // Calculate the position of the mouse in the map
            int x = (mousePos.X) / PixelSize;
            int y = (mousePos.Y) / PixelSize;
            
            // Change the value of the map
            if(x is >= 0 and < width && y is >= 0 and < height)
            {
                // map[x, y] = !map[x, y];
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    DrawCircle(x,y,DrawRadius);
                }
                else
                {
                    DrawCircle(x,y,DrawRadius,false);
                }
            }
        }

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (mapCopy[x, y])
                {
                    if(y+1 < height && !mapCopy[x, y+1])
                    {
                        map[x, y] = false;
                        map[x, y + 1] = true;
                    } else if (y + 1 < height && x - 1 >= 0 && !mapCopy[x - 1, y + 1])
                    {
                        map[x, y] = false;
                        map[x - 1, y + 1] = true;
                    }
                    else if (y + 1 < height && x + 1 < width && !mapCopy[x + 1, y + 1])
                    {
                        map[x, y] = false;
                        map[x + 1, y + 1] = true;
                    }
                }
                
                // y should be inverse
                SetColor(x, (y - height)* -1 - 1);
                
                mapCopy[x, y] = map[x, y];
            }
        }

        if (diagnoseTime)
        {
            updateClock.Stop();
            Console.WriteLine("Update time: " + updateClock.ElapsedMilliseconds + "ms");
            updateClock.Reset();
        }
    }

    private void SetColor(int x, int y)
    {
        colorMap[x, y] = map[x, y] ? pixelColor.ToColor() : backgroundColor.ToColor();
    }
    
    
    void DrawCircle(int xc, int yc, int radius, bool value = true)
    {
        for (int y1 = -radius; y1 <= radius; y1++)
        {
            for (int x1 = -radius; x1 <= radius; x1++)
            {
                if (x1 * x1 + y1 * y1 < radius * radius + radius)
                {
                    SetPixel(xc + x1, yc + y1, value);
                }
            }
        }
    }

    public void LineFrom(int x1, int y1, int x2, int y2)
    {
        int dx = x2 - x1;
        int dy = y2 - y1;
        int yi = 1;
        if (dy < 0)
        {
            yi = -1;
            dy = -dy;
        }
        int D = 2 * dy - dx;
        int y = y1;
        for (int x = x1; x <= x2; x++)
        {
            SetPixel(x, y);
            if (D > 0)
            {
                y += yi;
                D -= 2 * dx;
            }
            D += 2 * dy;
        }
    }
    
    public void SetPixel(int x, int y, bool value = true)
    {
        if (IsInRange(x, y))
        {
            map[x, y] = value;
            mapCopy[x, y] = value;
        }
    }
    
    public bool IsInRange(int x, int y)
    {
        return x is >= 0 and < width && y is >= 0 and < height;
    }
    
    public void Draw()
    {
        if (diagnoseTime)
        {
            drawClock.Start();
        }

        image = new Image(colorMap);
        texture.Update(image);

        window.Draw(sprite);
        image.Dispose();

        // for (int i = 0; i < 100; i++)
        // {
        //     for (int j = 0; j < 100; j++)
        //     {
        //         if (map[i, j])
        //         {
        //             pixelPos.X = i*PixelSize;
        //             pixelPos.Y = j*PixelSize;
        //             pixel.Position = pixelPos;
        //             window.Draw(pixel);
        //         }
        //     }
        // }
        
        
        if (diagnoseTime)
        {
            drawClock.Stop();
            Console.WriteLine("Draw time: " + drawClock.ElapsedMilliseconds + "ms");
            drawClock.Reset();
        }
    }

}
