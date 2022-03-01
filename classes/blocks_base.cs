// #include classes/block_options.cs
// #include helpers/string.cs

public class CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocksBase(string purpose = "")
  {
    m_blocks = new List<T>();
    m_purpose = purpose;
  }

  public void setup(string name,
                    bool visibleInTerminal  = false,
                    bool visibleInInventory = false,
                    bool visibleInToolBar   = false)
  {
    Dictionary<string, int> counetrs = new Dictionary<string, int>();
    string zeros = new string('0', count().ToString().Length);
    foreach (T block in m_blocks)
    {
      string blockPurpose = "";
      CBlockOptions options = new CBlockOptions(block);
      // per block type
      if(isAssignable<IMyShipConnector>())
      {
        IMyShipConnector blk = block as IMyShipConnector;
        blk.PullStrength = 1f;
        blk.CollectAll   = options.getValue("connector", "collectAll", false);
        blk.ThrowOut     = options.getValue("connector", "throwOut"  , false);
      } else if (isAssignable<IMyInteriorLight>())
      {
        IMyInteriorLight blk = block as IMyInteriorLight;
        blk.Radius = 10f;
        blk.Intensity = 10f;
        blk.Falloff = 3f;
        blk.Color = options.getValue("lamp", "color", Color.White);
      } else if (isAssignable<IMyConveyorSorter>())
      {
        IMyConveyorSorter blk = block as IMyConveyorSorter;
        blk.DrainAll = options.getValue("sorter", "drainAll", false);
      } else if (isAssignable<IMyLargeTurretBase>())
      {
        IMyLargeTurretBase blk = block as IMyLargeTurretBase;
        blk.EnableIdleRotation = true;
        blk.Elevation = 0f;
        blk.Azimuth = 0f;
      } else if (isAssignable<IMyAssembler>())
      {
        blockPurpose = "Master";
        if(options.getValue("assembler", "is_slave", false))
        {
          IMyAssembler blk = block as IMyAssembler;
          blk.CooperativeMode = true;
          blockPurpose = "Slave";
        }
      }
      // name
      string realPurpose = $"{getPurpose(options).Trim()} {blockPurpose}";
      if (!counetrs.ContainsKey(realPurpose)) { counetrs.Add(realPurpose, 0); }
      block.CustomName = TrimAllSpaces($"[{structureName}] {name} {realPurpose} {counetrs[realPurpose].ToString(zeros).Trim()}");
      counetrs[realPurpose]++;
      // visibility
      setupBlocksVisibility(block,
                            options.getValue("generic", "visibleInTerminal", visibleInTerminal),
                            options.getValue("generic", "visibleInInventory", visibleInInventory),
                            options.getValue("generic", "visibleInToolBar", visibleInToolBar));
    }
  }

  private string getPurpose(CBlockOptions options)
  {
    string result = options.getValue("generic", "purpose", m_purpose);
    return result != "" ? $" {result} " : " ";
  }

  private void setupBlocksVisibility(T block,
                                     bool vTerminal,
                                     bool vInventory,
                                     bool vToolBar)
  {
    IMySlimBlock sBlock = block.CubeGrid.GetCubeBlock(block.Position);
    block.ShowInTerminal = vTerminal && sBlock.IsFullIntegrity && sBlock.BuildIntegrity < 1f;
    block.ShowInToolbarConfig = vToolBar;
    if (block.HasInventory) { block.ShowInInventory = vInventory; }
  }


  public bool empty() { return m_blocks.Count == 0; }
  public int count() { return m_blocks.Count; }
  public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
  public bool isAssignable<U>() where U : class, IMyTerminalBlock
  {
    if (empty()) { return false; }
    return m_blocks[0] is U;
  }

  public List<T> blocks() { return m_blocks; }
  public string purpose() { return m_purpose; }
  protected void clear() { m_blocks.Clear(); }

  protected List<T> m_blocks;
  private string m_purpose;
}
