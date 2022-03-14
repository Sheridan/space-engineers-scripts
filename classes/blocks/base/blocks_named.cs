// #include classes/blocks/base/blocks_base.cs

public class CBlocksNamed<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocksNamed(string name, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_name = name; load(); }

  protected override bool checkBlock(T b)
  {
    return (m_loadOnlySameGrid ? b.IsSameConstructAs(self.Me) : true) && b.CustomName.Contains(m_name);
  }

  public string name() { return m_name; }
  private string m_name;
}
