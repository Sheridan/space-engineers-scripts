public static string TrimAllSpaces(string value)
{
  var newString = new StringBuilder();
  bool previousIsWhitespace = false;
  for (int i = 0; i < value.Length; i++)
  {
    if (Char.IsWhiteSpace(value[i]))
    {
      if (previousIsWhitespace)
      {
        continue;
      }
      previousIsWhitespace = true;
    }
    else
    {
      previousIsWhitespace = false;
    }

    newString.Append(value[i]);
  }
  return newString.ToString();
}
