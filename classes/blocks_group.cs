// #include classes/blocks_base.cs

public class CBlockGroup<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlockGroup(string groupName,
                     string purpose = "",
                     bool loadOnlySameGrid = true) : base(purpose)
  {
    m_groupName = groupName;
    refresh(loadOnlySameGrid);
  }

  public void refresh(bool loadOnlySameGrid = true)
  {
    clear();
    IMyBlockGroup group = self.GridTerminalSystem.GetBlockGroupWithName(m_groupName);
    if (loadOnlySameGrid) { group.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
    else                  { group.GetBlocksOfType<T>(m_blocks)                                   ; }
  }

  public string  groupName() { return m_groupName; }
  private string m_groupName;
}
