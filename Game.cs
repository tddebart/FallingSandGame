
public class Game
{
    public const uint width = 1000;
    public const uint height = 1000;
    private const string title = "Falling sand";
    private RenderWindow window;
    private readonly VideoMode mode = new(width, height);
    private readonly Font font = new("./Resources/Fonts/Arial.ttf");

    // fps counter stuff
    private int framesRendered;
    private string fps = "";
    private Text fpsText;
    private DateTime lastTime;
    private Clock deltaClock = new();
    
    private SandBox sandbox;

    public Game()
    {
        window = new RenderWindow(mode, title, Styles.Titlebar | Styles.Close);

        // window.SetVerticalSyncEnabled(true);
        window.Closed += (_, _) => window.Close();
        
        window.RequestFocus();

        fpsText = new Text(fps, font, 30);
        sandbox = new SandBox(window);
    }

    public void Run()
    {
        GuiImpl.Init(window);
        while (window.IsOpen)
        {
            HandleEvents();
            Update();
            Draw();
        }
    }

    private void HandleEvents()
    {
        window.DispatchEvents();
    }

    private void Update()
    {
        GuiImpl.Update(window, deltaClock.Restart().AsSeconds());
        sandbox.Update();
    }

    Vector3f color = new(0, 0, 0);
    private void Draw()
    {
        framesRendered++;
        
        window.Clear(Color.Black);

        if ((DateTime.Now - lastTime).TotalSeconds >= 1)
        {
            // one second has elapsed 
            fps = framesRendered.ToString();                     
            framesRendered = 0;            
            lastTime = DateTime.Now;
        }
        
        sandbox.Draw();
        
        ImGui.SetNextWindowPos(new Vector2(width-251,2));
        ImGui.SetNextWindowSize(new Vector2(250,150));
        ImGui.Begin("Main", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
        ImGui.Text("Hello World");
        ImGui.ColorEdit4("Background Color", ref SandBox.backgroundColor, ImGuiColorEditFlags.NoInputs);
        ImGui.ColorEdit4("Sand Color", ref SandBox.pixelColor, ImGuiColorEditFlags.NoInputs);
        ImGui.Spacing();
        ImGui.Spacing();
        ImGui.SliderInt("Draw radius", ref SandBox.DrawRadius, 1, 50, "%.0f");
        ImGui.End();
        // ImGui.ShowDemoWindow();

        GuiImpl.Render(window);
        fpsText.DisplayedString = fps;
        window.Draw(fpsText);
        
        window.Display();
    }
}