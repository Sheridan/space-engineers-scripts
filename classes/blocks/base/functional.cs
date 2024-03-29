// #include classes/blocks/base/terminal.cs

public class CFunctional<T> : CTerminal<T> where T : class, IMyFunctionalBlock
{
  public CFunctional(CBlocksBase<T> blocks) : base(blocks) {}
  public bool enable(bool target = true)
  {
    foreach (IMyFunctionalBlock b in m_blocks)
    {
      if(b.Enabled != target) { b.Enabled = target; }
    }
    return enabled() == target;
  }
  public bool disable() { return enable(false); }

  public bool enabled()
  {
    bool result = true;
    foreach (IMyFunctionalBlock b in m_blocks)
    {
      result = result && b.Enabled;
    }
    return result;
  }
}
