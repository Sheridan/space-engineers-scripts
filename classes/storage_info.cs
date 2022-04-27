// #include classes/blocks/container.cs
// #include classes/blocks/base/blocks_named.cs
// #include classes/display.cs
// #include helpers/human.cs

public class StorageInfo
{
  public StorageInfo(string storageName, string lcdName)
  {
    m_lastItemsTypes = 0;
    m_lcd = new CDisplay();
    m_lcd.addDisplays(lcdName);
    m_storage = new CContainer(new CBlocksNamed<IMyCargoContainer>(storageName));
  }

  public void update()
  {
    Dictionary<MyItemType, float> data = m_storage.items();
    if(m_lastItemsTypes > data.Count) { m_lcd.clear(); }
    m_lastItemsTypes = data.Count;
    int idx = 0;
    float maxVolume = m_storage.maxVolume();
    float volume    = m_storage.volume();
    m_lcd.echo_at($"Использовано {toHumanReadable(volume, EHRUnit.Volume)} из {toHumanReadable(maxVolume, EHRUnit.Volume)}: {volume/(maxVolume/100):f2}%", idx++);
    m_lcd.echo_at($"Общая масса: {toHumanReadable(m_storage.mass(), EHRUnit.Mass)}", idx++);
    foreach (KeyValuePair<MyItemType, float> i in data)
    {
      MyItemInfo inf = i.Key.GetItemInfo();
      string cnt = inf.UsesFractions ? $"Mass: {toHumanReadable(i.Value, EHRUnit.Mass)}" : $"{i.Value:f0} шт., Mass: {toHumanReadable(i.Value*inf.Mass, EHRUnit.Mass)}";
      m_lcd.echo_at($"{MyDefinitionId.Parse(i.Key.ToString()).SubtypeName}: {cnt}, Volume: {toHumanReadable(inf.Volume*i.Value, EHRUnit.Volume)}", idx++);
    }
  }

  private CDisplay m_lcd;
  private CContainer m_storage;
  private int m_lastItemsTypes;
}
