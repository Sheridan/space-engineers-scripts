// #include classes/blocks/base/functional.cs

public class CRefinery : CFunctional<IMyRefinery>
{
  public CRefinery(CBlocksBase<IMyRefinery> blocks) : base(blocks) { }
}
