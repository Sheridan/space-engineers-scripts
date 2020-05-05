// #include classes/blocks/terminal.cs

public class CFunctional<T> : CTerminal<T> where T : class, IMyTerminalBlock
{
  public CFunctional(CBlocksBase<T> blocks) : base(blocks) {}
  public void enable(bool enabled = true) { foreach (IMyFunctionalBlock block in m_blocks.blocks()) { if(block.Enabled != enabled) { block.Enabled = enabled; }}}
  public void disable() { enable(false); }
}
