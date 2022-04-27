public class CBlockOptions
{
  public CBlockOptions(IMyTerminalBlock block)
  {
    m_available = false;
    m_block = block;
    read();
  }

  private void read()
  {
    if (m_block.CustomData.Length > 0)
    {
      m_ini = new MyIni();
      MyIniParseResult result;
      m_available = m_ini.TryParse(m_block.CustomData, out result);
      if (!m_available) { debug(result.ToString()); }
    }
  }

  private void write() { m_block.CustomData = m_ini.ToString(); }

  private bool exists(string section, string name) { return m_available && m_ini.ContainsKey(section, name); }

  public string getValue(string section, string name, string defaultValue = "")
  {
    if (exists(section, name)) { return m_ini.Get(section, name).ToString().Trim(); }
    return defaultValue;
  }

  public bool getValue(string section, string name, bool defaultValue = true)
  {
    if (exists(section, name)) { return m_ini.Get(section, name).ToBoolean(); }
    return defaultValue;
  }

  public float getValue(string section, string name, float defaultValue = 0f)
  {
    if (exists(section, name)) { return float.Parse(m_ini.Get(section, name).ToString()); }
    return defaultValue;
  }

  public int getValue(string section, string name, int defaultValue = 0)
  {
    if (exists(section, name)) { return m_ini.Get(section, name).ToInt32(); }
    return defaultValue;
  }

  // R;G;B;A
  public Color getValue(string section, string name, Color defaultValue)
  {
    if (exists(section, name))
    {
      string[] color = m_ini.Get(section, name).ToString().Split(';');
      return new Color(float.Parse(color[0]), float.Parse(color[1]), float.Parse(color[2]), float.Parse(color[3]));
    }
    // m_ini.Set(section, name, defaultValue.ToString());
    return defaultValue;
  }

  IMyTerminalBlock m_block;
  private bool m_available;
  private MyIni m_ini;
}
