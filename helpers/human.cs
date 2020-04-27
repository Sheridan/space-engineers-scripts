public static string toHumanReadable(float value, string suffix = "")
{
  if (value < 1000)
  {
    return $"{value}{suffix}";
  }
  var exp = (int)(Math.Log(value) / Math.Log(1000));
  return $"{value / Math.Pow(1000, exp):f2}{("кМГТПЭ")[exp - 1]}{suffix}"; // "kMGTPE" "кМГТПЭ"
}

// public static string toHumanReadable(int value, string suffix = "")
