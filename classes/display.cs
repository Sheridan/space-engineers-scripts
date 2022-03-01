// #include classes/textsurface.cs

public class CDisplay : CTextSurface
{
  public CDisplay() : base()
  {
    // m_displays = new List<List<IMyTextPanel>>();
    m_initialized = false;
  }

  private void initSize(IMyTextPanel display)
  {
    if (!m_initialized)
    {
      debug($"{display.BlockDefinition.SubtypeName}");
      switch (display.BlockDefinition.SubtypeName)
      {
        case "LargeLCDPanelWide": setup(0.602f, 28, 87, 0.35f); break;
        case "LargeLCDPanel"    : setup(0.602f, 28, 44, 0.35f); break;
        default: setup(1f, 1, 1, 1f); break;
      }
    }
  }

  public void addDisplay(string name, int x, int y)
  {
    IMyTextPanel display = self.GridTerminalSystem.GetBlockWithName(name) as IMyTextPanel;
    // if (m_displays.Count <= x) { m_displays.Add(new List<IMyTextPanel>()); }
    // if (m_displays[x].Count <= y) { m_displays[x].Add(display); }
    // else { m_displays[x][y] = display; }
    initSize(display);
    addSurface(display as IMyTextSurface, x, y);
  }

  private bool m_initialized;
  // private string m_displayName;
  // private IMyTextPanel m_display;
  // private List<List<IMyTextPanel>> m_displays;
}
