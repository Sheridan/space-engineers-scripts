// #include classes/blocks/base/functional.cs

public class CShipTool : CFunctional<IMyShipToolBase>
{
  public CShipTool(CBlocksBase<IMyShipToolBase> blocks) : base(blocks) { }

  public bool on(bool target = true)
  {
    return enable(target) && activated(target);
  }

  public bool off() { return on(false); }
  public bool active() { return activated(true); }

  private bool activated(bool target)
  {
    bool result = true;
    foreach (IMyShipToolBase b in m_blocks.blocks())
    {
      result = result && b.IsActivated == target;
    }
    return result;
  }
}
