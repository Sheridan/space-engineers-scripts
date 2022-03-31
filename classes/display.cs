// #include classes/textsurface.cs
// #include classes/block_options.cs
// #include classes/blocks/base/blocks_named.cs

public class CDisplay : CTextSurface
{
  public CDisplay() : base()
  {}

  private void mineDimensions(IMyTextPanel display)
  {
    debug($"{display.BlockDefinition.SubtypeName}");
    switch (display.BlockDefinition.SubtypeName)
    {
      case "LargeLCDPanelWide"  : setup(0.602f, 28, 87, 0.35f); break;
      case "LargeLCDPanel"      : setup(0.602f, 28, 44, 0.35f); break;
      case "TransparentLCDLarge": setup(0.602f, 28, 44, 0.35f); break;
      case "TransparentLCDSmall": setup(0.602f, 26, 40,    4f); break;
      case "SmallTextPanel"     : setup(0.602f, 48, 48, 0.35f); break;
      default: setup(1f, 1, 1, 1f); break;
    }
  }

  public void addDisplays(string name)
  {
    CBlocksNamed<IMyTextPanel> displays = new CBlocksNamed<IMyTextPanel>(name);
    if(displays.empty()) { throw new System.ArgumentException("Не найдены дисплеи", name); }
    mineDimensions(displays.blocks()[0]);
    foreach(IMyTextPanel display in displays)
    {
      CBlockOptions options = new CBlockOptions(display);
      int x = options.getValue("display", "x", -1);
      int y = options.getValue("display", "y", -1);
      if(x<0 || y<0) { throw new System.ArgumentException("Не указаны координаты дисплея", display.CustomName); }
      addSurface(display as IMyTextSurface, x, y);
    }
    clear();
  }

}
