// #include classes/blocks/functional.cs

public class CShipTool : CFunctional<IMyShipToolBase>
{
  public CShipTool(CBlocksBase<IMyShipToolBase> blocks) : base(blocks) { }

  public bool on(bool target = true)
  {
    return enable(target) && checkActive(target);
  }

  public bool off() { return on(false); }
  public bool active() { return checkActive(true); }

  private bool checkActive(bool target)
  {
    bool result = true;
    foreach (IMyShipToolBase tool in m_blocks.blocks())
    {
      result = result && tool.IsActivated == target;
    }
    return result;
  }
}
