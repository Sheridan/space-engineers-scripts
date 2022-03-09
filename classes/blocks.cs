
// #include classes/blocks_base.cs

public class CBlocks<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocks(string name = "", string purpose = "", bool loadOnlySameGrid = true) : base(purpose)
  {
    refresh(name, loadOnlySameGrid);
  }

  public void refresh(string name = "", bool loadOnlySameGrid = true)
  {
    clear();
    if (loadOnlySameGrid) { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
    else                  { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks)                                   ; }
    if(name != string.Empty)
    {
      for (int i = count() - 1; i >= 0; i--)
      {
        if(!m_blocks[i].CustomName.Contains(name)) { removeBlockAt(i); }
      }
    }
  }
}
