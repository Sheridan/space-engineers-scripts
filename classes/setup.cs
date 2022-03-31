// #include classes/block_options.cs
// #include classes/blocks/base/terminal.cs
// #include helpers/string.cs

public class CSetup<T> : CTerminal<T> where T : class, IMyTerminalBlock
{
  public CSetup(CBlocksBase<T> blocks) : base(blocks)
  {
    m_zeros = new string('0', count().ToString().Length);
    m_counetrs = new Dictionary<string, int>();
  }

  public void setup(string name,
                    bool visibleInTerminal  = false,
                    bool visibleInInventory = false,
                    bool visibleInToolBar   = false)
  {
    echoMeBig(String.Join(Environment.NewLine, name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));
    foreach (T b in m_blocks)
    {
      string suffix = "";
      CBlockOptions options = new CBlockOptions(b);
      // per b type
           if(m_blocks.isAssignable<IMyShipConnector  >()) { suffix = setup(options, b as IMyShipConnector  ); }
      else if(m_blocks.isAssignable<IMyInteriorLight  >()) { suffix = setup(options, b as IMyInteriorLight  ); }
      else if(m_blocks.isAssignable<IMyConveyorSorter >()) { suffix = setup(options, b as IMyConveyorSorter ); }
      else if(m_blocks.isAssignable<IMyLargeTurretBase>()) { suffix = setup(options, b as IMyLargeTurretBase); }
      else if(m_blocks.isAssignable<IMyAssembler      >()) { suffix = setup(options, b as IMyAssembler      ); }
      else if(m_blocks.isAssignable<IMyReflectorLight >()) { suffix = setup(options, b as IMyReflectorLight ); }

      // name
      b.CustomName = generateName(name, suffix, loadPurpose(options));

      // visibility
      setupBlocksVisibility(b,
                            options.getValue("generic", "visibleInTerminal" , visibleInTerminal ),
                            options.getValue("generic", "visibleInInventory", visibleInInventory),
                            options.getValue("generic", "visibleInToolBar"  , visibleInToolBar  ));
    }
  }

  private string generateName(string name, string suffix, string purpose)
  {
    string baseName = TrimAllSpaces($"{name} {purpose} {suffix}");
    if(count() > 0)
    {
      if (!m_counetrs.ContainsKey(baseName)) { m_counetrs.Add(baseName, 0); }
      string order = m_counetrs[baseName].ToString(m_zeros).Trim();
      m_counetrs[baseName]++;
      return $"[{structureName}] {baseName} {order}";
    }
    return $"[{structureName}] {baseName}";
  }

  private string setup(CBlockOptions options, IMyShipConnector b)
  {
    b.PullStrength = 1f;
    b.CollectAll   = options.getValue("connector", "collectAll", false);
    b.ThrowOut     = options.getValue("connector", "throwOut"  , false);
    return string.Empty;
  }

  private string setup(CBlockOptions options, IMyInteriorLight b)
  {
    b.Radius = 100f;
    b.Intensity = 10f;
    b.Falloff = 3f;
    b.Color = options.getValue("lamp", "color", Color.White);
    return string.Empty;
  }

  private string setup(CBlockOptions options, IMyReflectorLight b)
  {
    b.Radius = 160f;
    b.Intensity = 100f;
    b.Falloff = 3f;
    b.Color = options.getValue("lamp", "color", Color.White);
    return string.Empty;
  }

  private string setup(CBlockOptions options, IMyConveyorSorter b)
  {
    b.DrainAll = options.getValue("sorter", "drainAll", false);
    return string.Empty;
  }

  private string setup(CBlockOptions options, IMyLargeTurretBase b)
  {
    b.EnableIdleRotation = true;
    b.Elevation = 0f;
    b.Azimuth = 0f;
    return string.Empty;
  }

  private string setup(CBlockOptions options, IMyAssembler b)
  {
    if(options.getValue("assembler", "is_slave", false))
    {
      b.CooperativeMode = true;
      return "Slave";
    }
    return "Master";
  }

  private string loadPurpose(CBlockOptions options) { return options.getValue("generic", "purpose", "").Trim(); }

  private void setupBlocksVisibility(T b,
                                     bool vTerminal,
                                     bool vInventory,
                                     bool vToolBar)
  {
    IMySlimBlock sB = b.CubeGrid.GetCubeBlock(b.Position);
    b.ShowInTerminal = vTerminal && sB.IsFullIntegrity && sB.BuildIntegrity < 1f;
    b.ShowInToolbarConfig = vToolBar;
    if (b.HasInventory) { b.ShowInInventory = vInventory; }
  }

  private string m_zeros;
  private Dictionary<string, int> m_counetrs;
}
