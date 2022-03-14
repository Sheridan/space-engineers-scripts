public static string currentTime()
{
  return System.DateTime.Now.ToString("HH:mm:ss");
}

public static string formatTimeSpan(TimeSpan ts)
{
  return ts.ToString(@"hh\:mm\:ss");
}
