public enum EBoolToString
{
  btsOnOff
}

public string boolToString(bool val, EBoolToString bsType = EBoolToString.btsOnOff)
{
  switch (bsType)
  {
    case EBoolToString.btsOnOff: return val ? "Вкл." : "Выкл.";
  }
  return val.ToString();
}
