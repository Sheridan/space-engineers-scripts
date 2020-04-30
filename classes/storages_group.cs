// #include classes/blocks_group.cs
// #include classes/recipes.cs

public class CStoragesGroup : CBlockGroup<IMyCargoContainer>
{
  public CStoragesGroup(string groupName,
                        string purpose = "") : base(groupName, purpose)
  {}

  public int countItems(EItemType itemType)
  {
    CComponentItem result = new CComponentItem(itemType);
    MyItemType miType = result.asMyItemType();
    foreach (IMyCargoContainer container in blocks())
    {
      result.appendAmount(container.GetInventory().GetItemAmount(miType).ToIntSafe());
    }
    return result.amount();
  }
}
