// #include classes/blocks/base/blocks_base.cs

public class CEntity<T> where T : class, IMyEntity
{
  public CEntity(CBlocksBase<T> blocks)
  {
    m_blocks = blocks;
    inventory = new CInventory(m_blocks);
  }
  public bool empty() { return m_blocks.empty(); }
  public int  count() { return m_blocks.count(); }
  public T    first() { return m_blocks.first(); }

  public IEnumerator GetEnumerator()
  {
    foreach(T i in m_blocks)
    {
      yield return i;
    }
  }

  protected CBlocksBase<T> m_blocks;
  public CInventory inventory;
}
