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
public enum EBoolToString
{
  btsOnOff
}
public string boolToString(bool val, EBoolToString bsType)
{
  switch (bsType)
  {
    case EBoolToString.btsOnOff: return val ? "Вкл." : "Выкл.";
  }
  return val.ToString();
}
public CBlockGroup<IMyShipDrill> drills;
public CBlockGroup<IMyPistonBase> pistons;
public CBlockGroup<IMyMotorStator> rotors;
public CBlockGroup<IMyGyro> gyroscopes;
const float drillRPM = 0.005f;
public void initGroups()
{
  rotors = new CBlockGroup<IMyMotorStator>("[Крот] Роторы буров", "Роторы");
  drills = new CBlockGroup<IMyShipDrill>("[Крот] Буры", "Буры");
  pistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни буров", "Поршни");
  gyroscopes = new CBlockGroup<IMyGyro>("[Крот] Гироскопы бура", "Гироскопы автогоризонта");
}
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
public void expandPistons(CBlockGroup<IMyPistonBase> pistons,
                          float length,
                          float velocity,
                          float force,
                          int stackSize = 1)
{
  float realLength = length / stackSize;
  float realVelocity = velocity / stackSize;
  float currentPosition = 0;
  foreach (IMyPistonBase piston in pistons.blocks())
  {
    switch (piston.Status)
    {
      case PistonStatus.Stopped:
      case PistonStatus.Retracted:
      case PistonStatus.Retracting:
      case PistonStatus.Extended:
        {
          if (piston.CurrentPosition < realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = 0f;
            piston.MaxLimit = realLength;
            piston.SetValue<float>("MaxImpulseAxis", force);
            piston.SetValue<float>("MaxImpulseNonAxis", force);
            piston.Extend();
          }
        }
        break;
    }
    currentPosition += piston.CurrentPosition;
  }
  currentPosition = currentPosition / pistons.count();
  lcd.echo($"[{pistons.purpose()}] Выдвигаются до {currentPosition:f2}->{realLength:f2}");
}
public void retractPistons(CBlockGroup<IMyPistonBase> pistons,
                          float minLength,
                          float velocity,
                          float force,
                          int stackSize = 1)
{
  float realLength = minLength / stackSize;
  float realVelocity = velocity / stackSize;
  float currentPosition = 0;
  foreach (IMyPistonBase piston in pistons.blocks())
  {
    switch (piston.Status)
    {
      case PistonStatus.Stopped:
      case PistonStatus.Extended:
      case PistonStatus.Extending:
      case PistonStatus.Retracted:
        {
          if (piston.CurrentPosition > realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = realLength;
            piston.MaxLimit = 10f;
            piston.SetValue<float>("MaxImpulseAxis", force);
            piston.SetValue<float>("MaxImpulseNonAxis", force);
            piston.Retract();
          }
        }
        break;
    }
    currentPosition += piston.CurrentPosition;
  }
  currentPosition = currentPosition / pistons.count();
  lcd.echo($"[{pistons.purpose()}] Задвигаются до {currentPosition:f2}->{realLength:f2}");
}
public void playSound(string name)
{
  soundBlock.SelectedSound = name;
  soundBlock.Play();
}
float lastAngle;
public bool canStep(float angle, float currentAngle, float delta = 1f)
{
  float minAngle = angle - delta;
  float maxAngle = angle + delta;
  bool result = currentAngle > minAngle && currentAngle < maxAngle;
  if (result && lastAngle != angle)
  {
    lastAngle = angle;
    return true;
  }
  return false;
}
CDisplay lcd;
IMySoundBlock soundBlock;
const float pistonDrillVelocity = 0.1f;
const float pistonUpVelocity = 0.5f;
const float pistonDrillForce = 1000000f;
const float pistonUpForce = 500000f;
const int pistonsInStack = 3;
float maxDrillLength;
float pistonStep;
IMyProgrammableBlock autoHorizont;
public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  autoHorizont = GridTerminalSystem.GetBlockWithName("[Крот] ПрБ Атоматический горизонт") as IMyProgrammableBlock;
  pistonStep = blockSize;
  maxDrillLength = pistonStep*11;
  lcd = new CDisplay();
  lcd.addDisplay("[Крот] Дисплей логов бурения 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей логов бурения 1", 1, 0);
  initGroups();
  soundBlock = GridTerminalSystem.GetBlockWithName("[Крот] Динамик") as IMySoundBlock;
  return "Управление бурением";
}
public void main(string argument, UpdateType updateSource)
{
  if(!parseArgumets(argument))
  {
    IMyMotorStator rotor = rotors.blocks()[0];
    if(rotor.TargetVelocityRPM > 0f)
    {
      float currentAngle = rotor.Angle * 180 / (float)Math.PI;
      if (canStep(10f, currentAngle) || canStep(130f, currentAngle) || canStep(250f, currentAngle))
      {
        bool pistonsAtMaxLength = true;
        foreach (IMyPistonBase piston in pistons.blocks())
        {
          pistonsAtMaxLength = pistonsAtMaxLength && piston.CurrentPosition >= maxDrillLength/pistonsInStack;
        }
        if(!pistonsAtMaxLength) { pistonsStep(); }
        else                    { stopWork   (); }
      }
    }
  }
}
float pistonPosition = 0f;
public bool parseArgumets(string argument)
{
  if(argument.Length == 0) { return false; }
  if      (argument == "go")           { startWork(); }
  else if (argument == "stop")         { stopWork (); }
  else if (argument.Contains("power")) { turnDrills(argument.Contains("_on")); }
  else if (argument.Contains("rotor")) { turnRotors(argument.Contains("_start")); }
  else if (argument.Contains("piston"))
  {
    if      (argument.Contains("_up"  )) { pistonsUp()  ; }
    else if (argument.Contains("_step")) { pistonsStep(); }
  }
  return true;
}
public void pistonsStep(int steps = 1)
{
  playSound("Operation Alarm");
  pistonPosition += pistonStep*steps;
  expandPistons(pistons, pistonPosition, pistonDrillVelocity, pistonDrillForce, pistonsInStack);
}
public void pistonsUp()
{
  retractPistons(pistons, 0f, pistonUpVelocity, pistonUpForce, pistonsInStack);
  pistonPosition = 0f;
}
public void turnRotors(bool enable)
{
  lcd.echo($"Вращение ({rotors.count()} шт.): {boolToString(enable, EBoolToString.btsOnOff)}");
  foreach (IMyMotorStator rotor in rotors.blocks())
  { rotor.TargetVelocityRPM = enable ? drillRPM : 0f; }
}
public void turnDrills(bool enable)
{
  lcd.echo($"Питание буров ({drills.count()} шт.): {boolToString(enable, EBoolToString.btsOnOff)}");
  foreach (IMyShipDrill drill in drills.blocks()) { drill.Enabled = enable; }
}
public void stopWork()
{
  turnDrills(false);
  turnRotors(false);
  pistonsUp();
  autoHorizont.TryRun("stop");
}
public void startWork()
{
  turnDrills(true);
  turnRotors(true);
  autoHorizont.TryRun("start");
}
