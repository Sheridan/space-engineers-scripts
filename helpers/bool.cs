public enum EBoolToString
{
  btsOnOff,
  btsOpenClose,
  btsYesNo
}

public string boolToString(bool val, EBoolToString bsType = EBoolToString.btsOnOff)
{
  switch (bsType)
  {
    case EBoolToString.btsOnOff    : return val ? "Вкл."  : "Выкл.";
    case EBoolToString.btsOpenClose: return val ? "Откр." : "Закр.";
    case EBoolToString.btsYesNo    : return val ? "Да"    : "Нет";
  }
  return val.ToString();
}
