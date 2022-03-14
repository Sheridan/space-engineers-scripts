// #include classes/blocks/base/functional.cs

public class CProjector : CFunctional<IMyProjector>
{
  public CProjector(CBlocksBase<IMyProjector> blocks) : base(blocks) { }
  public bool projecting()
  {
    bool result = true;
    foreach (IMyProjector b in m_blocks.blocks())
    {
      result = result && b.IsProjecting;
    }
    return result;
  }

  public int totalBlocks()
  {
    int result = 0;
    foreach (IMyProjector b in m_blocks.blocks())
    {
      result = result + b.TotalBlocks;
    }
    return result;
  }

  public int remainingBlocks()
  {
    int result = 0;
    foreach (IMyProjector b in m_blocks.blocks())
    {
      result = result + b.RemainingBlocks;
    }
    return result;
  }

  public int buildableBlocks()
  {
    int result = 0;
    foreach (IMyProjector b in m_blocks.blocks())
    {
      result = result + b.BuildableBlocksCount;
    }
    return result;
  }

  public int weldedBlocks() { return totalBlocks() - remainingBlocks(); }
}
