// #include classes/block_options.cs

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
      CBlockOptions options = new CBlockOptions(block);
      // name
      string realPurpose = options.getValue("generic", "purpose", m_purpose);
      if (realPurpose != "") { realPurpose = $" {realPurpose} "; } else { realPurpose = " "; }
      if (!counetrs.ContainsKey(realPurpose)) { counetrs.Add(realPurpose, 0); }
      block.CustomName = $"[{structureName}] {name}{realPurpose}{counetrs[realPurpose].ToString(zeros)}";
      counetrs[realPurpose]++;
      // visibility
      setupBlocksVisibility(block,
                            options.getValue("generic", "visibleInTerminal", visibleInTerminal),
                            options.getValue("generic", "visibleInInventory", visibleInInventory),
                            options.getValue("generic", "visibleInToolBar", visibleInToolBar));

    }
  }
  private void setupBlocksVisibility(T block,
                                     bool vTerminal,
                                     bool vInventory,
                                     bool vToolBar)
  {
    block.ShowInTerminal = vTerminal;
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
