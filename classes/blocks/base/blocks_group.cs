// #include classes/blocks/base/blocks.cs

public class CBlockGroup<T> : CBlocks<T> where T : class, IMyTerminalBlock
{
  public CBlockGroup(string groupName, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_groupName = groupName; load(); }

  protected override void load()
  {
    self.GridTerminalSystem.GetBlockGroupWithName(m_groupName).GetBlocksOfType<T>(m_blocks, x => checkBlock(x));
  }

  public string groupName() { return m_groupName; }
  private string m_groupName;
}
