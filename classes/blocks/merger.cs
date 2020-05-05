// #include classes/blocks/functional.cs

public class CMerger : CFunctional<IMyShipMergeBlock>
{
  public CMerger(CBlocksBase<IMyShipMergeBlock> blocks) : base(blocks) { }

  public bool connect(bool enabled = true)
  {
    enable(enabled);
    bool result = true;
    foreach (IMyShipMergeBlock merger in m_blocks.blocks())
    {
      result = result && merger.Enabled == enabled;
    }
    return result;
  }
  public bool disconnect() { return connect(false); }

}
