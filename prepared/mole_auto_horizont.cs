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
public class CBlockGroup<T> where T : class, IMyTerminalBlock
{
  public CBlockGroup(string name, string purpose, bool loadOnlySameGrid = true)
  {
    m_purpose = purpose;
    m_groupName = name;
    refresh(loadOnlySameGrid);
  }
  public void refresh(bool loadOnlySameGrid = true)
  {
    m_blocks = new List<T>();
    IMyBlockGroup group = self.GridTerminalSystem.GetBlockGroupWithName(m_groupName);
    if (loadOnlySameGrid) { group.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
    else                  { group.GetBlocksOfType<T>(m_blocks)                                  ; }
  }
  public bool empty() { return m_blocks.Count == 0; }
  public int count() { return m_blocks.Count; }
  public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
  public bool isAssignable<U>() where U : class, IMyTerminalBlock
  {
    if(empty()) { return false; }
    return m_blocks[0] is U;
  }
  public List<T> blocks   () { return m_blocks;    }
  public string  purpose  () { return m_purpose;   }
  public string  groupName() { return m_groupName; }
  private List<T> m_blocks;
  private string m_purpose;
  private string m_groupName;
}
public class CBlockStatusDisplay : CDisplay
{
  public CBlockStatusDisplay() : base() {}
  private string getFunctionaBlocksStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if(!group.isAssignable<IMyFunctionalBlock>()) { return ""; }
    string result = "";
    int pOn = 0;
    foreach (IMyFunctionalBlock block in group.blocks())
    {
      if (block.Enabled) { pOn++; }
    }
    result += $"Power: {pOn} ";
    return result;
  }
  private string getRotorsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyMotorStator>()) { return ""; }
    string result = "";
    List<string> rpm = new List<string>();
    List<string> angle = new List<string>();
    foreach (IMyMotorStator block in group.blocks())
    {
      float angleGrad = block.Angle * 180 / (float)Math.PI;
      rpm.Add($"{block.TargetVelocityRPM:f2}");
      angle.Add($"{angleGrad:f2}°");
    }
    result += $"Angle: {string.Join(":", angle)} "
            + $"RPM: {string.Join(":", rpm)} ";
    return result;
  }
  private string getDrillsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if(!group.isAssignable<IMyShipDrill>()) { return ""; }
    string result = "";
    IMyInventory inventory;
    int volume = 0;
    int mass = 0;
    int items = 0;
    foreach (IMyShipDrill block in group.blocks())
    {
      inventory = block.GetInventory();
      volume += inventory.CurrentVolume.ToIntSafe();
      mass += inventory.CurrentMass.ToIntSafe();
      items += inventory.ItemCount;
    }
    result += $"VMI: {toHumanReadable(volume, "Л")}:{toHumanReadable(mass, "Кг")}:{toHumanReadable(items)} ";
    return result;
  }
  private string getPistonsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyPistonBase>()) { return ""; }
    string result = "";
    List<string> positions = new List<string>();
    int statusStopped = 0;
    int statusExtending = 0;
    int statusExtended = 0;
    int statusRetracting = 0;
    int statusRetracted = 0;
    foreach (IMyPistonBase block in group.blocks())
    {
      switch(block.Status)
      {
        case PistonStatus.Stopped: statusStopped++; break;
        case PistonStatus.Extending: statusExtending++; break;
        case PistonStatus.Extended: statusExtended++; break;
        case PistonStatus.Retracting: statusRetracting++; break;
        case PistonStatus.Retracted: statusRetracted++; break;
      }
      positions.Add($"{block.CurrentPosition:f2}");
    }
    result += $"SeErR: {statusStopped}:{statusExtending}:{statusExtended}:{statusRetracting}:{statusRetracted} "
            + $"Pos: {string.Join(":", positions)} ";
    return result;
  }
  private string getMergersStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyShipMergeBlock>()) { return ""; }
    string result = "";
    int connected = 0;
    foreach (IMyShipMergeBlock block in group.blocks())
    {
      if (block.IsConnected) { connected++; }
    }
    result += $"Connected: {connected} ";
    return result;
  }
  private string getConnectorsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyShipConnector>()) { return ""; }
    string result = "";
    int statusUnconnected = 0;
    int statusConnectable = 0;
    int statusConnected = 0;
    foreach (IMyShipConnector block in group.blocks())
    {
      switch (block.Status)
      {
        case MyShipConnectorStatus.Unconnected: statusUnconnected++; break;
        case MyShipConnectorStatus.Connectable: statusConnectable++; break;
        case MyShipConnectorStatus.Connected: statusConnected++; break;
      }
    }
    result += $"UcC: {statusUnconnected}:{statusConnectable}:{statusConnected} ";
    return result;
  }
  private string getProjectorsStatus<T>(CBlockGroup<T> group) where T : class, IMyTerminalBlock
  {
    if (!group.isAssignable<IMyProjector>()) { return ""; }
    string result = "";
    int projecting = 0;
    List<string> blocksTotal = new List<string>();
    List<string> blocksRemaining = new List<string>();
    List<string> blocksBuildable = new List<string>();
    foreach (IMyProjector block in group.blocks())
    {
      if (block.IsProjecting) { projecting++; }
      blocksTotal.Add($"{block.TotalBlocks}");
      blocksRemaining.Add($"{block.RemainingBlocks}");
      blocksBuildable.Add($"{block.BuildableBlocksCount}");
    }
    result += $"Pr: {projecting} "
           + $"B: {string.Join(":", blocksBuildable)} "
           + $"R: {string.Join(":", blocksRemaining)} "
           + $"T: {string.Join(":", blocksTotal)} "
           ;
    return result;
  }
  public void showStatus<T>(CBlockGroup<T> group, int position) where T : class, IMyTerminalBlock
  {
    string result = $"[{group.subtypeName()}] {group.purpose()} ";
    if(!group.empty())
    {
      result += $"({group.count()}) "
             + getPistonsStatus<T>(group)
             + getConnectorsStatus<T>(group)
             + getMergersStatus<T>(group)
             + getProjectorsStatus<T>(group)
             + getDrillsStatus<T>(group)
             + getRotorsStatus<T>(group)
             + getFunctionaBlocksStatus<T>(group)
             ;
    }
    else
    {
      result += $" Группа {group.groupName()} пуста";
    }
    echo_at(result, position);
  }
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
    int minColumn = x * m_maxColumns;
    int maxColumn = minColumn + m_maxColumns;
    int minLine = y * m_maxLines + y;
    int maxLine = minLine + m_maxLines;
    for (int lineNum = minLine; lineNum <= maxLine; lineNum++)
    {
      if (m_text.Count <= lineNum) { break; }
      string line = m_text[lineNum];
      int substringLength = line.Length > maxColumn ? m_maxColumns : line.Length - minColumn;
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
  }
  private int m_maxLines;
  private int m_maxColumns;
  private float m_fontSize;
  private float m_padding;
  private List<string> m_text;
  private List<List<IMyTextSurface>> m_surfaces;
}
public static string toHumanReadable(float value, string suffix = "")
{
  if (value < 1000)
  {
    return $"{value}{suffix}";
  }
  var exp = (int)(Math.Log(value) / Math.Log(1000));
  return $"{value / Math.Pow(1000, exp):f2}{("кМГТПЭ")[exp - 1]}{suffix}"; // "kMGTPE" "кМГТПЭ"
}
class CShipController
{
  public CShipController(IMyShipController controller)
  {
    m_controller = controller;
  }
  public CShipController(string name)
  {
    m_controller = self.GridTerminalSystem.GetBlockWithName(name) as IMyShipController;
  }
  public void enableAutoHorizont(CBlockGroup<IMyGyro> gyroscopes)
  {
    if (autoHorizontIsEnabled()) { return; }
    m_autoHorizontGyroscopes = gyroscopes;
    foreach (IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks())
    {
      gyroscope.GyroOverride = true;
    }
  }
  public void disableAutoHorizont()
  {
    if (!autoHorizontIsEnabled()) { return; }
    foreach (IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks())
    {
      gyroscope.Yaw = 0;
      gyroscope.Roll = 0;
      gyroscope.Pitch = 0;
      gyroscope.GyroOverride = false;
    }
    m_autoHorizontGyroscopes = null;
  }
  public void autoHorizont(float yaw)
  {
    if(!autoHorizontIsEnabled()) { return; }
    Vector3D normGravity = Vector3D.Normalize(m_controller.GetNaturalGravity());
    foreach(IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks())
    {
      gyroscope.Yaw   = yaw;
      gyroscope.Roll  = (float)-normGravity.Dot(m_controller.WorldMatrix.Left);
      gyroscope.Pitch = (float) normGravity.Dot(m_controller.WorldMatrix.Forward);
    }
  }
  public bool autoHorizontIsEnabled() { return m_autoHorizontGyroscopes != null; }
  private IMyShipController m_controller;
  CBlockGroup<IMyGyro> m_autoHorizontGyroscopes;
}
public static double angleBetweenVectors(Vector3D a, Vector3D b)
{
  return MathHelper.ToDegrees(Math.Acos(a.Dot(b) / (a.Length() * b.Length())));
}
CBlockGroup<IMyGyro> gyroscopes;
CBlockStatusDisplay lcd;
CShipController controller;
const float rotorDrillVelocity = 0.003f;
public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplay("[Крот] Дисплей статуса строительства 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей статуса строительства 1", 1, 0);
  controller = new CShipController("[Крот] Д.У. Буров");
  gyroscopes = new CBlockGroup<IMyGyro>("[Крот] Гироскопы бура", "Гироскопы автогоризонта");
  return "Атоматический горизонт";
}
public void main(string argument, UpdateType updateSource)
{
  if (argument == "")
  {
    controller.autoHorizont(rotorDrillVelocity);
  }
  else
  {
    if (argument == "start")
    {
      controller.enableAutoHorizont(gyroscopes);
      Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }
    else
    {
      controller.disableAutoHorizont();
      Runtime.UpdateFrequency = UpdateFrequency.None;
    }
  }
}
