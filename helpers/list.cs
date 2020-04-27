public List<T> oddEvenleList<T>(List<T> list)
{
  List<T> result = new List<T>();
  int n = list.Count;
  for (int i = 0; i < n; i++)
  {
    if (i % 2 == 0) { result.Add(list[i]); }
    else { result.Insert(0, list[i]); }
  }
  return result;
}

public List<T> shuffleList<T>(List<T> list)
{
  Random rng = new Random();
  int n = list.Count;
  while (n > 1)
  {
    n--;
    int k = rng.Next(n + 1);
    T value = list[k];
    list[k] = list[n];
    list[n] = value;
  }
  return list;
}
