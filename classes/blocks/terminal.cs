// #include classes/textsurface.cs

public class CTerminal<T> where T : class, IMyTerminalBlock
{
  public CTerminal(CBlocksBase<T> blocks) { m_blocks = blocks; }

  public void listProperties(CTextSurface lcd)
  {
    if(m_blocks.count() == 0) { return; }
    List<ITerminalProperty> properties = new List<ITerminalProperty>();
    m_blocks.blocks()[0].GetProperties(properties);
    foreach (var property in properties)
    {
      lcd.echo($"id: {property.Id}, type: {property.TypeName}");
    }
  }

  public void listActions(CTextSurface lcd)
  {
    if(m_blocks.count() == 0) { return; }
    List<ITerminalAction> actions = new List<ITerminalAction>();
    m_blocks.blocks()[0].GetActions(actions);
    foreach (var action in actions)
    {
      lcd.echo($"id: {action.Id}, name: {action.Name}");
    }
  }

  void showInTerminal(bool show = true) { foreach (T block in m_blocks.blocks()) { if(block.ShowInTerminal != show) { block.ShowInTerminal = show; }}}
  void hideInTerminal() { showInTerminal(false); }

  void showInToolbarConfig(bool show = true) { foreach (T block in m_blocks.blocks()) { if(block.ShowInToolbarConfig != show) { block.ShowInToolbarConfig = show; }}}
  void hideInToolbarConfig() { showInToolbarConfig(false); }

  void showInInventory(bool show = true) { foreach (T block in m_blocks.blocks()) { if(block.ShowInInventory != show) { block.ShowInInventory = show; }}}
  void hideInInventory() { showInInventory(false); }

  void showOnHUD(bool show = true) { foreach (T block in m_blocks.blocks()) { if(block.ShowOnHUD != show) { block.ShowOnHUD = show; }}}
  void hideOnHUD() { showOnHUD(false); }

  protected CBlocksBase<T> m_blocks;
}
