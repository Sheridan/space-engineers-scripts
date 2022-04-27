// #include classes/blocks/base/blocks_base.cs

public class CBlocksTyped<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocksTyped(string subTypeName, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_subTypeName = subTypeName; load(); }

  protected override bool checkBlock(T b)
  {
    return (m_loadOnlySameGrid ? self.Me.IsSameConstructAs(b) : true) && b.BlockDefinition.ToString().Contains(m_subTypeName);
  }

  public string subTypeName() { return m_subTypeName; }
  private string m_subTypeName;
}
