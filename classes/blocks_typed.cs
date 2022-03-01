// #include classes/blocks_base.cs

public class CBlocksTyped<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocksTyped(string subTypeName,
                      string purpose = "",
                      bool loadOnlySameGrid = true) : base(purpose)
  {
    m_subTypeName = subTypeName;
    refresh(loadOnlySameGrid);
  }

  public void refresh(bool loadOnlySameGrid = true)
  {
    clear();
    if (loadOnlySameGrid) {
           self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => (x.IsSameConstructAs(self.Me) &&
                                                                      x.BlockDefinition.ToString().Contains(m_subTypeName))); }
    else { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x =>  x.BlockDefinition.ToString().Contains(m_subTypeName)); }
  }

  public string subTypeName() { return m_subTypeName; }
  private string m_subTypeName;
}
