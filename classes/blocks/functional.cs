// #include classes/blocks/terminal.cs

public class CFunctional<T> : CTerminal<T> where T : class, IMyTerminalBlock
{
  public CFunctional(CBlocksBase<T> blocks) : base(blocks) {}
  public bool enable(bool target = true)
  {
    foreach (IMyFunctionalBlock block in m_blocks.blocks())
    {
      if(block.Enabled != target) { block.Enabled = target; }
    }
    return enabled() == target;
  }
  public bool disable() { return enable(false); }

  public bool enabled()
  {
    bool result = true;
    foreach (IMyFunctionalBlock block in m_blocks.blocks())
    {
      result = result && block.Enabled;
    }
    return result;
  }
}
