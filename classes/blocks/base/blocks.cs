// #include classes/blocks/base/blocks_base.cs

public class CBlocks<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocks(bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { load(); }
}
