string structureName;
static Program self;
public float blockSize;
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
  setupMe(program());
}
public void Main(string argument, UpdateType updateSource) { main(argument, updateSource); }
public class CTextSurface
{
  public CTextSurface()
  {
    m_text = new List<string>();
    m_surfaces = new List<List<IMyTextSurface>>();
  }
  public void setSurface(IMyTextSurface surface, float fontSize, int maxLines, int maxColumns, float padding = 0)
  {
    setup(fontSize, maxLines, maxColumns, padding);
    addSurface(surface, 0, 0);
  }
  public void addSurface(IMyTextSurface surface, int x, int y)
  {
    if(countSurfacesX()  <= x) { m_surfaces.Add(new List<IMyTextSurface>()); }
    if(countSurfacesY(x) <= y) { m_surfaces[x].Add(surface); }
    else                       { m_surfaces[x][y] = surface; }
    setup();
  }
  public void setup(float fontSize, int maxLines, int maxColumns, float padding)
  {
    m_fontSize = fontSize;
    m_maxLines = maxLines;
    m_maxColumns = maxColumns;
    m_padding = padding;
    setup();
  }
  private void setup()
  {
    foreach(List<IMyTextSurface> sfList in m_surfaces)
    {
      foreach (IMyTextSurface surface in sfList)
      {
        surface.ContentType = ContentType.TEXT_AND_IMAGE;
        surface.Font = "Monospace";
        surface.FontColor = new Color(255, 255, 255);
        surface.BackgroundColor = new Color(0, 0, 0);
        surface.FontSize = m_fontSize;
        surface.Alignment = TextAlignment.LEFT;
        surface.TextPadding = m_padding;
      }
    }
    clear();
  }
  public void clear()
  {
    foreach (List<IMyTextSurface> sfList in m_surfaces)
    {
      foreach (IMyTextSurface surface in sfList)
      {
        surface.WriteText("", false);
      }
    }
  }
  private bool surfaceExists(int x, int y)
  {
    return y < countSurfacesY(x);
  }
  private bool unknownTypeEcho(string text)
  {
    if (m_maxLines == 0 && surfaceExists(0,0)) { m_surfaces[0][0].WriteText(text + '\n', true); return true; }
    return false;
  }
  private int countSurfacesX(     ) { return m_surfaces.Count; }
  private int countSurfacesY(int x) { return x < countSurfacesX() ? m_surfaces[x].Count : 0; }
  public void echo(string text)
  {
    if(!unknownTypeEcho(text))
    {
      if (m_text.Count > m_maxLines*countSurfacesY(0)) { m_text.RemoveAt(0); }
      m_text.Add(text);
    }
    echoText();
  }
  public void echo_last(string text)
  {
    if (!unknownTypeEcho(text))
    {
      m_text[m_text.Count - 1] = text;
      echoText();
    }
  }
  public void echo_at(string text, int lineNum)
  {
    if (!unknownTypeEcho(text))
    {
      if (lineNum >= m_text.Count)
      {
        for(int i = m_text.Count; i <= lineNum; i++) { m_text.Add("\n"); }
      }
      m_text[lineNum] = text;
      echoText();
    }
  }
  private void updateSurface(int x, int y)
  {
    int minColumn = x * m_maxColumns;
    int maxColumn = minColumn + m_maxColumns;
    int minLine = y * m_maxLines+y;
    int maxLine = minLine + m_maxLines;
    for(int lineNum = minLine; lineNum <= maxLine; lineNum++)
    {
      if(m_text.Count <= lineNum) { break; }
      string line = m_text[lineNum];
      int substringLength = line.Length > maxColumn ? m_maxColumns : line.Length - minColumn;
      if(substringLength > 0)
      {
        m_surfaces[x][y].WriteText(line.Substring(minColumn, substringLength) + '\n', true);
      }
      else
      {
        m_surfaces[x][y].WriteText("\n", true);
      }
    }
  }
  private void echoText()
  {
    clear();
    for(int x = 0; x < countSurfacesX(); x++)
    {
      for (int y = 0; y < countSurfacesY(x); y++)
      {
        updateSurface(x, y);
      }
    }
  }
  private int m_maxLines;
  private int m_maxColumns;
  private float m_fontSize;
  private float m_padding;
  private List<string> m_text;
  private List<List<IMyTextSurface>> m_surfaces;
}
public class CDisplay : CTextSurface
{
  public CDisplay() : base()
  {
    m_initialized = false;
  }
  private void initSize(IMyTextPanel display)
  {
    if (!m_initialized)
    {
      switch (display.BlockDefinition.SubtypeName)
      {
        case "LargeLCDPanelWide": setup(0.602f, 28, 87, 0.35f); break;
        default: setup(1f, 0, 0, 0f); break;
      }
    }
  }
  public void addDisplay(string name, int x, int y)
  {
    IMyTextPanel display = self.GridTerminalSystem.GetBlockWithName(name) as IMyTextPanel;
    initSize(display);
    addSurface(display as IMyTextSurface, x, y);
  }
  private bool m_initialized;
}
CDisplay lcd;
public string program()
{
  lcd = new CDisplay();
  lcd.addDisplay("[Крот] Дисплей логов бурения 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей логов бурения 1", 1, 0);
  lcd.addDisplay("[Крот] Дисплей статуса бурения 0", 0, 1);
  lcd.addDisplay("[Крот] Дисплей статуса бурения 1", 1, 1);
  return "Тестирование дисплеев";
}
public void main(string argument, UpdateType updateSource)
{
  for (int i = 0; i < 100; i++)
  {
    lcd.echo($"{i.ToString("0000")} 678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
    lcd.echo($"{i.ToString("0000")}    10        20        30        40        50        60        70        80        90       100       110       120       130       140       150       160       170       180       190       200");
  }
}
