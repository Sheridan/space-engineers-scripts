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
    foreach (T b in m_blocks.blocks())
    {
      string suffix = "";
      CBlockOptions options = m_blocks.options(b);
      // per b type
           if(m_blocks.isAssignable<IMyShipConnector  >()) { suffix = setup(options, b as IMyShipConnector  ); }
      else if(m_blocks.isAssignable<IMyInteriorLight  >()) { suffix = setup(options, b as IMyInteriorLight  ); }
      else if(m_blocks.isAssignable<IMyConveyorSorter >()) { suffix = setup(options, b as IMyConveyorSorter ); }
      else if(m_blocks.isAssignable<IMyLargeTurretBase>()) { suffix = setup(options, b as IMyLargeTurretBase); }
      else if(m_blocks.isAssignable<IMyAssembler      >()) { suffix = setup(options, b as IMyAssembler      ); }

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
    if (!m_counetrs.ContainsKey(baseName)) { m_counetrs.Add(baseName, 0); }
    string order = count() > 1 ? m_counetrs[baseName].ToString(m_zeros).Trim() : "";
    m_counetrs[baseName]++;
    return TrimAllSpaces($"[{structureName}] {baseName} {order}");
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
    b.Radius = 10f;
    b.Intensity = 10f;
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
