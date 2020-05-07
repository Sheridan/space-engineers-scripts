// #include classes/blocks/functional.cs

public class CTank : CFunctional<IMyGasTank>
{
  public CTank(CBlocksBase<IMyGasTank> blocks) : base(blocks) { }

  public bool enableStockpile(bool enabled = true)
  {
    bool result = true;
    foreach (IMyGasTank tank in m_blocks.blocks())
    {
      if(tank.Stockpile != enabled) { tank.Stockpile = enabled; }
      result = result && tank.Stockpile == enabled;
    }
    return result;
  }
  public bool disableStockpile() { return enableStockpile(false); }

}
