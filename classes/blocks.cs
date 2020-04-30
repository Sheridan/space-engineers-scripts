
// #include classes/blocks_base.cs

public class CBlocks<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocks(string purpose = "", bool loadOnlySameGrid = true) : base(purpose)
  {
    refresh(loadOnlySameGrid);
  }

  public void refresh(bool loadOnlySameGrid = true)
  {
    clear();
    if (loadOnlySameGrid) { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
    else                  { self.GridTerminalSystem.GetBlocksOfType<T>(m_blocks)                                   ; }
  }
}
