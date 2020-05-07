public enum EHRUnit
{
  None,
  Mass,
  Volume,
  Power,
  PowerCapacity
}
public static string hrSuffix(EHRUnit unit)
{
  switch (unit)
  {
    case EHRUnit.None          : return "шт.";
    case EHRUnit.Mass          : return "г.";
    case EHRUnit.Volume        : return "м³";
    case EHRUnit.Power         : return "Вт.";
    case EHRUnit.PowerCapacity : return "ВтЧ.";
  }
  return "";
}
public static string toHumanReadable(float value, EHRUnit unit = EHRUnit.None)
{
  string suffix = hrSuffix(unit);
  if(value < 1000) { return $"{value}{suffix}"; }
  int exp = (int)(Math.Log(value) / Math.Log(1000));
  return $"{value / Math.Pow(1000, exp):f2}{("кМГТПЭ")[exp - 1]}{suffix}"; // "kMGTPE" "кМГТПЭ"
}

// public static string toHumanReadable(int value, string suffix = "")
