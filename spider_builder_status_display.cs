// #include classes/main.cs
// #include classes/blockstatus_display.cs
// #include parts/spider_builder.cs

CBlockStatusDisplay lcd;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplay("[Паук] Дисплей статуса строительства 0", 0, 0);
  lcd.addDisplay("[Паук] Дисплей статуса строительства 1", 1, 0);
  initGroups();
  return "Отображение статуса строительства";
}

public void main(string argument, UpdateType updateSource)
{
  lcd.showStatus<IMyShipMergeBlock>(weldersMergers, 0);
  lcd.showStatus<IMyPistonBase>(weldersMergersPistons, 1);
  lcd.showStatus<IMyShipMergeBlock>(supportMergers, 2);
  lcd.showStatus<IMyPistonBase>(supportMergersPistons, 3);
  lcd.showStatus<IMyPistonBase>(mainPistons, 4);
  lcd.showStatus<IMyShipWelder>(welders, 5);
  lcd.showStatus<IMyProjector>(projectors, 6);
}
