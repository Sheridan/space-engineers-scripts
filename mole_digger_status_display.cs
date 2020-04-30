// #include classes/main.cs
// #include classes/blockstatus_display.cs
// #include parts/mole_digger.cs

CBlockStatusDisplay lcd;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplay("[Крот] Дисплей статуса бурения 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей статуса бурения 1", 1, 0);
  initGroups();
  return "Отображение статуса бурения";
}

public void main(string argument, UpdateType updateSource)
{
  lcd.showStatus<IMyShipDrill>(drills, 0);
  lcd.showStatus<IMyPistonBase>(pistons, 1);
  lcd.showStatus<IMyMotorStator>(rotors, 2);
  lcd.showStatus<IMyGyro>(gyroscopes, 3);
}
