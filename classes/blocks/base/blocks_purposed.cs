// #include classes/blocks/base/blocks_base.cs

public class CBlocksPurposed<T> : CBlocksBase<T> where T : class, IMyEntity
{
  public CBlocksPurposed(string purpose, bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { m_purpose = purpose; load(); }

  protected override bool checkBlock(T b)
  {
    CBlockOptions options = new CBlockOptions(b);
    return (m_loadOnlySameGrid ? self.Me.IsSameConstructAs(b) : true) && options.getValue("generic", "purpose", "") == m_purpose;
  }

  public string purpose() { return m_purpose; }
  private string m_purpose;
}
