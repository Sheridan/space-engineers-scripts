public class CXYCollection<T> : IEnumerable
{
  public CXYCollection()
  {
    m_data = new Dictionary<int,Dictionary<int,T>>();
  }

  public T get(int x, int y)
  {
    if(exists(x,y))
    {
      return m_data[x][y];
    }
    return default(T);
  }

  public void set(int x, int y, T data)
  {
    debug($"set {x}:{y}");
    if(!m_data.ContainsKey(x)) { m_data[x] = new Dictionary<int,T>(); }
    m_data[x][y] = data;
  }

  public bool exists(int x, int y)
  {
    return m_data.ContainsKey(x) && m_data[x].ContainsKey(y);
  }

  public int count()
  {
    int result = 0;
    foreach(KeyValuePair<int,Dictionary<int,T>> i in m_data)
    {
      result += i.Value.Count;
    }
    return result;
  }

  public int countX() { return m_data.Count; }
  public int countY() { return empty() ? 0 : m_data[0].Count; }
  public bool empty() { return countX() == 0; }

  public IEnumerator GetEnumerator()
  {
    foreach(KeyValuePair<int,Dictionary<int,T>> i in m_data)
    {
      foreach(KeyValuePair<int,T> j in i.Value)
      {
        yield return j.Value;
      }
    }
  }

  private Dictionary<int,Dictionary<int,T>> m_data;
}
