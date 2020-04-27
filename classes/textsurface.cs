public class CTextSurface
{
  public CTextSurface()
  {
    // m_surface = surface;
    m_text = new List<string>();
    m_surfaces = new List<List<IMyTextSurface>>();
    // addSurface(surface, 0, 0);
    // clear();
  }

  public void setSurface(IMyTextSurface surface, float fontSize, int maxLines, int maxColumns, float padding = 0)
  {
    setup(fontSize, maxLines, maxColumns, padding);
    addSurface(surface, 0, 0);
  }

  public void addSurface(IMyTextSurface surface, int x, int y)
  {
    if (countSurfacesX() <= x) { m_surfaces.Add(new List<IMyTextSurface>()); }
    if (countSurfacesY(x) <= y) { m_surfaces[x].Add(surface); }
    else { m_surfaces[x][y] = surface; }
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
    foreach (List<IMyTextSurface> sfList in m_surfaces)
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
    // return x < m_surfaces.Count && y < m_surfaces[x].Count;
  }

  private bool unknownTypeEcho(string text)
  {
    if (m_maxLines == 0 && surfaceExists(0, 0)) { m_surfaces[0][0].WriteText(text + '\n', true); return true; }
    return false;
  }

  private int countSurfacesX() { return m_surfaces.Count; }
  private int countSurfacesY(int x) { return x < countSurfacesX() ? m_surfaces[x].Count : 0; }

  public void echo(string text)
  {
    if (!unknownTypeEcho(text))
    {
      if (m_text.Count > m_maxLines * countSurfacesY(0)) { m_text.RemoveAt(0); }
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
    for (int x = 0; x < countSurfacesX(); x++)
    {
      for (int y = 0; y < countSurfacesY(x); y++)
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
  private List<List<IMyTextSurface>> m_surfaces;
}
