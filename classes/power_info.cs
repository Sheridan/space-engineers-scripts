// #include classes/display.cs
// #include helpers/human.cs
// #include classes/blocks/base/terminal.cs
// #include classes/blocks/base/blocks.cs
// #include classes/block_power_info.cs
// #include classes/min_current_max.cs

public class CPowerInfo
{
  public CPowerInfo(string lcdNameProducing, string lcdNameConsuming)
  {
    m_blocks = new CTerminal<IMyTerminalBlock>(new CBlocks<IMyTerminalBlock>());
    m_lcdProducers = new CDisplay();
    m_lcdProducers.addDisplays(lcdNameProducing);
    m_lcdConsumers = new CDisplay();
    m_lcdConsumers.addDisplays(lcdNameConsuming);
  }

  private void updateBlocksInfo()
  {
    reset();
    foreach(IMyTerminalBlock b in m_blocks)
    {
      string name = b.DefinitionDisplayNameText;
      CBlockPowerInfo pi = new CBlockPowerInfo(b);
      if(pi.canProduce()) { addPowerBlock(m_producers, name, pi.currentProduce(), pi.maxProduce()); }
      if(pi.canConsume()) { addPowerBlock(m_consumers, name, pi.currentConsume(), pi.maxConsume()); }
    }
    if(m_producers.Count < m_lastProdCout) { m_lcdProducers.clear(); } m_lastProdCout = m_producers.Count;
    if(m_consumers.Count < m_lastConsCout) { m_lcdConsumers.clear(); } m_lastConsCout = m_consumers.Count;
  }

  public void update()
  {
    updateBlocksInfo();
    drawInfo(m_lcdProducers, m_producers, "Генерация");
    drawInfo(m_lcdConsumers, m_consumers, "Потребление");
  }

  private void drawInfo(CDisplay to, Dictionary<string, SMinCurrentMax<float>> from, string title)
  {
    int j = 0;
    to.echo_at(title, j++);
    foreach(KeyValuePair<string, SMinCurrentMax<float>> i in from)
    {
      to.echo_at($"[{i.Key}:{i.Value.count}] {toHumanReadable(i.Value.current, EHRUnit.Power)} of {toHumanReadable(i.Value.max, EHRUnit.Power)}", j++);
    }
  }

  private void addPowerBlock(Dictionary<string, SMinCurrentMax<float>> to, string name, float cr, float mx)
  {
    if(!to.ContainsKey(name)) { to[name] = new SMinCurrentMax<float>(0f, cr, mx); to[name].count++; }
    else
    {
      SMinCurrentMax<float> mcm = to[name];
      mcm.current += cr;
      mcm.max     += mx;
      mcm.count   ++;
    }
  }

  private void reset()
  {
    m_producers = new Dictionary<string, SMinCurrentMax<float>>();
    m_consumers = new Dictionary<string, SMinCurrentMax<float>>();
  }

  private CTerminal<IMyTerminalBlock> m_blocks;

  private Dictionary<string, SMinCurrentMax<float>> m_producers;
  private Dictionary<string, SMinCurrentMax<float>> m_consumers;

  private CDisplay m_lcdConsumers;
  private CDisplay m_lcdProducers;

  private int m_lastConsCout;
  private int m_lastProdCout;
}
