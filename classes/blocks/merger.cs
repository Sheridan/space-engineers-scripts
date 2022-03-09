// #include classes/blocks/functional.cs

public class CMerger : CFunctional<IMyShipMergeBlock>
{
  public CMerger(CBlocksBase<IMyShipMergeBlock> blocks) : base(blocks) { }

  public bool connect(bool target = true)
  {
    enable(target);
    return connected() == target;
  }
  public bool disconnect() { return connect(false); }
  public bool connected ()
  {
    if(!enabled()) { return false; }
    bool result = true;
    foreach (IMyShipMergeBlock blk in m_blocks.blocks())
    {
      result = result && blk.IsConnected;
    }
    return result;
  }

}
