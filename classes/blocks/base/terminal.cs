// #include classes/textsurface.cs
// #include classes/blocks/base/cube.cs

public class CTerminal<T> : CCube<T> where T : class, IMyTerminalBlock
{
  public CTerminal(CBlocksBase<T> blocks) : base(blocks) {}

  public void listProperties(CTextSurface lcd)
  {
    if(empty()) { return; }
    List<ITerminalProperty> properties = new List<ITerminalProperty>();
    first().GetProperties(properties);
    foreach (var property in properties)
    {
      lcd.echo($"id: {property.Id}, type: {property.TypeName}");
    }
  }

  public void listActions(CTextSurface lcd)
  {
    if(empty()) { return; }
    List<ITerminalAction> actions = new List<ITerminalAction>();
    first().GetActions(actions);
    foreach (var action in actions)
    {
      lcd.echo($"id: {action.Id}, name: {action.Name}");
    }
  }

  void showInTerminal(bool show = true) { foreach (T b in m_blocks) { if(b.ShowInTerminal != show) { b.ShowInTerminal = show; }}}
  void hideInTerminal() { showInTerminal(false); }

  void showInToolbarConfig(bool show = true) { foreach (T b in m_blocks) { if(b.ShowInToolbarConfig != show) { b.ShowInToolbarConfig = show; }}}
  void hideInToolbarConfig() { showInToolbarConfig(false); }

  void showInInventory(bool show = true) { foreach (T b in m_blocks) { if(b.ShowInInventory != show) { b.ShowInInventory = show; }}}
  void hideInInventory() { showInInventory(false); }

  void showOnHUD(bool show = true) { foreach (T b in m_blocks) { if(b.ShowOnHUD != show) { b.ShowOnHUD = show; }}}
  void hideOnHUD() { showOnHUD(false); }


}
