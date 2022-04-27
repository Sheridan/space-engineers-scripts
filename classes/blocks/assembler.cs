// #include classes/blocks/base/functional.cs
// #include classes/blocks/inventory.cs

public class CAssembler : CFunctional<IMyAssembler>
{
  public CAssembler(CBlocksBase<IMyAssembler> blocks) : base(blocks)
  {
    m_master = null;
    foreach (IMyAssembler b in m_blocks)
    {
      if(b.CustomName.Contains("Master")) { m_master = b; }
    }
    if(m_master != null) { m_blocks.removeBlock(m_master); }
  }

  public bool producing()
  {
    // if(m_master != null && m_master.IsProducing) { return true; }
    foreach (IMyAssembler b in m_blocks) { if(b.IsProducing || !b.IsQueueEmpty) { return true; } }
    return false;
  }

  public void produce(string bpDefinition, int amount)
  {
    if(m_master != null) { m_master.AddQueueItem(MyDefinitionId.Parse(bpDefinition), (double)amount); }
    else
    {
      int realAmount = (int)Math.Ceiling((double)amount/m_blocks.count());
      foreach (IMyAssembler b in m_blocks) { b.AddQueueItem(MyDefinitionId.Parse(bpDefinition), (double)realAmount); }
    }
  }

  public void clear()
  {
    if(m_master != null) { m_master.ClearQueue(); }
    foreach (IMyAssembler b in m_blocks) { b.ClearQueue(); }
  }

  private IMyAssembler m_master;

}
