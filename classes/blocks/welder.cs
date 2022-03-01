// #include classes/blocks/functional.cs

public class CWelder : CFunctional<IMyShipWelder>
{
  public CWelder(CBlocksBase<IMyShipWelder> blocks) : base(blocks) { }
}
