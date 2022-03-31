// #include classes/blocks/base/functional.cs

public class CTank : CFunctional<IMyGasTank>
{
  public CTank(CBlocksBase<IMyGasTank> blocks) : base(blocks) { }

  public bool enableStockpile(bool enabled = true)
  {
    bool result = true;
    foreach (IMyGasTank b in m_blocks)
    {
      if(b.Stockpile != enabled) { b.Stockpile = enabled; }
      result = result && b.Stockpile == enabled;
    }
    return result;
  }
  public bool disableStockpile() { return enableStockpile(false); }

}
