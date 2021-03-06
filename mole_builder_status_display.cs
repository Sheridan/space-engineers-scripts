// #include classes/main.cs
// #include classes/blockstatus_display.cs
// #include parts/mole_builder.cs

CBlockStatusDisplay lcd;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplay("[Крот] Дисплей статуса строительства 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей статуса строительства 1", 1, 0);
  initGroups();
  return "Отображение статуса строительства";
}

public void main(string argument, UpdateType updateSource)
{
  lcd.showStatus<IMyShipMergeBlock>(weldersMergers, 0);
  lcd.showStatus<IMyPistonBase>(weldersMergersPistons, 1);
  lcd.showStatus<IMyShipMergeBlock>(supportMergers, 2);
  lcd.showStatus<IMyPistonBase>(supportMergersPistons, 3);
  lcd.showStatus<IMyShipMergeBlock>(logisticMergers, 4);
  lcd.showStatus<IMyPistonBase>(logisticPistons, 5);
  lcd.showStatus<IMyPistonBase>(mainPistons, 6);
  lcd.showStatus<IMyShipConnector>(logisticConnectors, 7);
  lcd.showStatus<IMyShipConnector>(mainConnectors, 8);
  lcd.showStatus<IMyShipWelder>(welders, 9);
  lcd.showStatus<IMyProjector>(projectors, 10);
}
