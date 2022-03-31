// #include helpers/string.cs

public class CBlocksBase<T> where T : class, IMyEntity
{
  public CBlocksBase(bool loadOnlySameGrid = true) { m_blocks = new List<T>(); m_loadOnlySameGrid = loadOnlySameGrid; }

  public    bool          empty        (     ) { return count() == 0;   }
  public    int           count        (     ) { return m_blocks.Count; }
  public    List<T>       blocks       (     ) { return m_blocks;       }
  protected void          clear        (     ) { m_blocks.Clear();      }
  public    void          removeBlock  (T b  ) { m_blocks.Remove(b);    }
  public    void          removeBlockAt(int i) { m_blocks.RemoveAt(i);  }
  public    T             first        (     ) { return m_blocks[0];    }

  public bool isAssignable<U>() where U : class, IMyEntity
  {
    if (empty()) { return false; }
    return m_blocks[0] is U;
  }

  public IEnumerator GetEnumerator()
  {
    foreach(T i in m_blocks)
    {
      yield return i;
    }
  }

  protected virtual void load      (   ) { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => checkBlock(x)); }
  protected virtual bool checkBlock(T b)
  {
    return m_loadOnlySameGrid ? self.Me.IsSameConstructAs(b as IMyTerminalBlock) : true;
  }

  protected List<T> m_blocks;
  protected bool    m_loadOnlySameGrid;
}
