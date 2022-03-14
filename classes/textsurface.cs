// #include classes/xycollection.cs

public class CTextSurface
{
  public CTextSurface()
  {
    m_text = new List<string>();
    m_surfaces = new CXYCollection<IMyTextSurface>();
  }

  public void setSurface(IMyTextSurface surface, float fontSize, int maxLines, int maxColumns, float padding = 0)
  {
    setup(fontSize, maxLines, maxColumns, padding);
    addSurface(surface, 0, 0);
  }

  public void addSurface(IMyTextSurface surface, int x, int y)
  {
    m_surfaces.set(x, y, setup(surface));
  }

  protected void setup(float fontSize, int maxLines, int maxColumns, float padding)
  {
    m_fontSize   = fontSize;
    m_maxLines   = maxLines;
    m_maxColumns = maxColumns;
    m_padding    = padding;
  }

  private IMyTextSurface setup(IMyTextSurface surface)
  {
    surface.ContentType = ContentType.TEXT_AND_IMAGE;
    surface.Font = "Monospace";
    surface.FontColor = new Color(255, 255, 255);
    surface.BackgroundColor = new Color(0, 0, 0);
    surface.FontSize = m_fontSize;
    surface.Alignment = TextAlignment.LEFT;
    surface.TextPadding = m_padding;
    return surface;
  }

  public void clear()
  {
    clearSurfaces();
    m_text.Clear();
  }

  private void clearSurfaces()
  {
    foreach (IMyTextSurface surface in m_surfaces) { surface.WriteText("", false); }
  }

  private bool unknownTypeEcho(string text)
  {
    if (m_maxLines == 0 && m_surfaces.exists(0, 0)) { m_surfaces.get(0,0).WriteText(text + '\n', true); return true; }
    return false;
  }

  public void echo(string text)
  {
    if (!unknownTypeEcho(text))
    {
      if (m_text.Count > m_maxLines * m_surfaces.countY()) { m_text.RemoveAt(0); }
      m_text.Add(text);
    }
    echoText();
  }

  // public void echo_n(string text) { m_surface.WriteText(text       , true) ; }

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
        for (int i = m_text.Count; i <= lineNum; i++) { m_text.Add("\n"); }
      }
      m_text[lineNum] = text;
      echoText();
    }
  }

  private void updateSurface(int x, int y)
  {
    // debug($"surface: {x}:{y}");
    int minColumn = x * m_maxColumns;
    int maxColumn = minColumn + m_maxColumns;
    int minLine = y * m_maxLines + y;
    int maxLine = minLine + m_maxLines;
    for (int lineNum = minLine; lineNum <= maxLine; lineNum++)
    {
      if (m_text.Count <= lineNum) { break; }
      string line = m_text[lineNum];
      int substringLength = line.Length > maxColumn ? m_maxColumns : line.Length - minColumn;
      // debug($"substring: {line.Length} -> {minColumn}:{substringLength}");
      if (substringLength > 0)
      {
        m_surfaces.get(x,y).WriteText(line.Substring(minColumn, substringLength) + '\n', true);
      }
      else
      {
        m_surfaces.get(x,y).WriteText("\n", true);
      }
    }
  }

  private void echoText()
  {
    clearSurfaces();
    for (int x = 0; x < m_surfaces.countX(); x++)
    {
      for (int y = 0; y < m_surfaces.countY(); y++)
      {
        updateSurface(x, y);
      }
    }
    // echo_n(string.Join("\n", m_text));
  }

  // public void lines_test()
  // {
  //   clear();
  //   for(int i = 1; i <= (m_maxLines > 0 ? m_maxLines : 100); i++)
  //   {
  //     echo($"Text line # {i}");
  //   }
  // }

  private int m_maxLines;
  private int m_maxColumns;
  private float m_fontSize;
  private float m_padding;
  private List<string> m_text;
  CXYCollection<IMyTextSurface> m_surfaces;
}
