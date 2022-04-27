// #include classes/blocks/base/blocks_base.cs
// #include classes/components.cs

public class CBlocksWithInventory<T> : CBlocksBase<T> where T : class, IMyTerminalBlock
{
  public CBlocksWithInventory(bool loadOnlySameGrid = true) : base(loadOnlySameGrid) { load(); }

  protected override bool checkBlock(T b)
  {
    return (m_loadOnlySameGrid ? self.Me.IsSameConstructAs(b) : true) &&
    isAssignable<IMyTerminalBlock>(b) && (b as IMyTerminalBlock).HasInventory;
  }

  public int items(EItemType itemType)
  {
    CComponentItem result = new CComponentItem(itemType);
    MyItemType miType = result.asMyItemType();
    foreach (IMyTerminalBlock b in m_blocks)
    {
      result.appendAmount(b.GetInventory().GetItemAmount(miType).ToIntSafe());
    }
    return result.amount();
  }

  public Dictionary<MyItemType, float> items()
  {
    Dictionary<MyItemType, float> result = new Dictionary<MyItemType, float>();
    foreach (IMyTerminalBlock b in m_blocks)
    {
      List<MyInventoryItem> ci = new List<MyInventoryItem>();
      b.GetInventory().GetItems(ci, x => true);
      foreach(MyInventoryItem i in ci)
      {
        if(!result.ContainsKey(i.Type)) { result.Add(i.Type, (float)i.Amount); }
        else { result[i.Type] += (float)i.Amount; }
      }
    }
    return result;
  }

  public float maxVolume()
  {
    float result = 0;
    foreach (IMyTerminalBlock b in m_blocks)
    {
      result += (float)b.GetInventory().MaxVolume;
    }
    return result;
  }

  public float volume()
  {
    float result = 0;
    foreach (IMyTerminalBlock b in m_blocks)
    {
      result += (float)b.GetInventory().CurrentVolume;
    }
    return result;
  }

  public float mass()
  {
    float result = 0;
    foreach (IMyTerminalBlock b in m_blocks)
    {
      result += (float)b.GetInventory().CurrentMass;
    }
    return result;
  }

}
