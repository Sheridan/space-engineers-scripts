// #include classes/main.cs
// #include classes/display.cs

CDisplay lcd;

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CDisplay();
  // lcd.addDisplay("[Крот] Дисплей логов бурения 0", 0, 0);
  // lcd.addDisplay("[Крот] Дисплей логов бурения 1", 1, 0);
  // lcd.addDisplay("[Крот] Дисплей статуса бурения 0", 0, 1);
  // lcd.addDisplay("[Крот] Дисплей статуса бурения 1", 1, 1);
  lcd.addDisplay("[Бур] Дисплей Статус 0", 0, 0);
  lcd.addDisplay("[Бур] Дисплей Статус 1", 1, 0);
  lcd.addDisplay("[Бур] Дисплей Статус 2", 2, 0);
  return "Тестирование дисплеев";
}

public void main(string argument, UpdateType updateSource)
{
  for (int i = 0; i < 100; i++)
  {
    lcd.echo($"{i.ToString("0000")} 678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
    lcd.echo($"{i.ToString("0000")}    10        20        30        40        50        60        70        80        90       100       110       120       130       140       150       160       170       180       190       200");
  }
  // int i = 0;
  // for(int x = 0; x < 100; x++)
  // {
  //   for (int y = 0; y < 100; y++)
  //   {
  //     lcd
  //     i++;
  //   }
  // }
}
