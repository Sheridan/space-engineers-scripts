// #include classes/blocks/base/entity.cs

public class CCube<T> : CEntity<T> where T : class, IMyCubeBlock
{
  public CCube(CBlocksBase<T> blocks) : base(blocks) {}
}
