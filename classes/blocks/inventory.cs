// #include classes/components.cs

public class CInventory
{
  CInventory()
  {
    m_inventoryes = new List<IMyInventory>();
  }

  CInventory(CBlocksBase<IMyEntity> blocks)
  {
    m_inventoryes = new List<IMyInventory>();
    foreach (IMyEntity b in blocks)
    {
      inventory.addInvertoryes(b);
    }
  }

  void addInvertoryes(IMyEntity b)
  {
    for(int i=0; i<b.InventoryCount; i++)
    {
      m_inventoryes.Add(b.GetInventory(i))
    }
  }

  public int items(EItemType itemType)
  {
    CComponentItem result = new CComponentItem(itemType);
    MyItemType miType = result.asMyItemType();
    foreach (IMyInventory i in m_inventoryes)
    {
      result.appendAmount(i.GetItemAmount(miType).ToIntSafe());
    }
    return result.amount();
  }

  public Dictionary<MyItemType, float> items()
  {
    Dictionary<MyItemType, float> result = new Dictionary<MyItemType, float>();
    foreach (IMyInventory i in m_inventoryes)
    {
      List<MyInventoryItem> ci = new List<MyInventoryItem>();
      i.GetItems(ci, x => true);
      foreach(MyInventoryItem j in ci)
      {
        if(!result.ContainsKey(j.Type)) { result.Add(j.Type, (float)j.Amount); }
        else { result[j.Type] += (float)j.Amount; }
      }
    }
    return result;
  }

  public float maxVolume()
  {
    float result = 0;
    foreach (IMyInventory i in m_inventoryes)
    {
      result += (float)i.MaxVolume;
    }
    return result;
  }

  public float volume()
  {
    float result = 0;
    foreach (IMyInventory i in m_inventoryes)
    {
      result += (float)i.CurrentVolume;
    }
    return result;
  }

  public float mass()
  {
    float result = 0;
    foreach (IMyInventory i in m_inventoryes)
    {
      result += (float)i.CurrentMass;
    }
    return result;
  }


  private List<IMyInventory> m_inventoryes;
}