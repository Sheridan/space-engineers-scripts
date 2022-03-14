// #include classes/blocks/base/functional.cs

public class CSpeaker : CFunctional<IMySoundBlock>
{
  public CSpeaker(CBlocksBase<IMySoundBlock> blocks) : base(blocks) { }

  public void play() { foreach (IMySoundBlock b in m_blocks.blocks()) { b.Play(); } }
  public void stop() { foreach (IMySoundBlock b in m_blocks.blocks()) { b.Stop(); } }
}
