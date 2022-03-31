// #include classes/blocks/base/functional.cs

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
    foreach (IMyShipMergeBlock b in m_blocks)
    {
      result = result && b.IsConnected;
    }
    return result;
  }

}
