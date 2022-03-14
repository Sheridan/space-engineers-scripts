// #include classes/blocks/base/functional.cs

public class CCollector : CFunctional<IMyCollector>
{
  public CCollector(CBlocksBase<IMyCollector> blocks) : base(blocks) { }
}
