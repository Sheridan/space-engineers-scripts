// #include classes/blocks/functional.cs

public class CProjector : CFunctional<IMyProjector>
{
  public CProjector(CBlocksBase<IMyProjector> blocks) : base(blocks) { }
  public bool projecting()
  {
    bool result = true;
    foreach (IMyProjector blk in m_blocks.blocks())
    {
      result = result && blk.IsProjecting;
    }
    return result;
  }

  public int totalBlocks()
  {
    int result = 0;
    foreach (IMyProjector blk in m_blocks.blocks())
    {
      result = result + blk.TotalBlocks;
    }
    return result;
  }

  public int remainingBlocks()
  {
    int result = 0;
    foreach (IMyProjector blk in m_blocks.blocks())
    {
      result = result + blk.RemainingBlocks;
    }
    return result;
  }

  public int weldedBlocks()
  {
    return totalBlocks() - remainingBlocks();
  }
}
