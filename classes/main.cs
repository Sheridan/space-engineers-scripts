// #include classes/block_options.cs

static string structureName;
static Program self;
static float blockSize;
static CBlockOptions prbOptions;

public void setupMe(string scriptName)
{
  Me.CustomName = $"[{structureName}] ПрБ {scriptName}";
  setupMeSurface(0, 2f);
  setupMeSurface(1, 5f);
  Me.GetSurface(0).WriteText(scriptName.Replace(" ", "\n"));
  Me.GetSurface(1).WriteText(structureName);
}

public void setupMeSurface(int i, float fontSize)
{
  IMyTextSurface surface = Me.GetSurface(i);
  surface.ContentType = ContentType.TEXT_AND_IMAGE;
  surface.Font = "Monospace";
  surface.FontColor = new Color(255, 255, 255);
  surface.BackgroundColor = new Color(0, 0, 0);
  surface.FontSize = fontSize;
  surface.Alignment = TextAlignment.CENTER;
}

public static void debug(string text) { self.Echo(text); }

public Program()
{
  self = this;
  structureName = Me.CubeGrid.CustomName;
  blockSize = Me.CubeGrid.GridSize;
  prbOptions = new CBlockOptions(Me);
  setupMe(program());
}

public void Main(string argument, UpdateType updateSource) { main(argument, updateSource); }
