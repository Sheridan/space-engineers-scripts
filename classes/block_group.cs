// enum EBlockType
// {
//   btMerger,
//   btConnector,
//   btWelder,
//   btPiston
// }
public class CBlockGroup<T> where T : class, IMyTerminalBlock
{
  public CBlockGroup(string name, string purpose, bool loadOnlySameGrid = true)
  {
    m_purpose = purpose;
    m_groupName = name;
    refresh(loadOnlySameGrid);
  }

  public void refresh(bool loadOnlySameGrid = true)
  {
    m_blocks = new List<T>();
    IMyBlockGroup group = self.GridTerminalSystem.GetBlockGroupWithName(m_groupName);
    if (loadOnlySameGrid) { group.GetBlocksOfType<T>(m_blocks, x => x.IsSameConstructAs(self.Me)); }
    else                  { group.GetBlocksOfType<T>(m_blocks)                                  ; }
  }

  public bool empty() { return m_blocks.Count == 0; }
  public int count() { return m_blocks.Count; }
  public string subtypeName() { return empty() ? "N/A" : m_blocks[0].DefinitionDisplayNameText; }
  public bool isAssignable<U>() where U : class, IMyTerminalBlock
  {
    if(empty()) { return false; }
    return m_blocks[0] is U;
  }

  public List<T> blocks() { return m_blocks; }
  public string purpose() { return m_purpose; }
  public string groupName() { return m_groupName; }

  private List<T> m_blocks;
  private string m_purpose;
  private string m_groupName;
}
