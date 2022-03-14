// #include classes/main.cs
// #include classes/blockstatus_display.cs
// #include classes/blocks/base/blocks.cs
// #include classes/blocks/base/blocks_typed.cs

CBlockStatusDisplay lcd;
public CBlocks<IMyShipDrill> drills;
public CBlocks<IMyShipConnector> connectors;
public CBlocks<IMyCargoContainer> storage;
public CBlocks<IMyThrust> thrusters;
public CBlocks<IMyGyro> gyroscopes;
public CBlocks<IMyBatteryBlock> battaryes;

public void initGroups()
{
  drills = new CBlocks<IMyShipDrill>();
  connectors = new CBlocks<IMyShipConnector>();
  storage = new CBlocks<IMyCargoContainer>();
  thrusters = new CBlocks<IMyThrust>();
  gyroscopes = new CBlocks<IMyGyro>();
  battaryes = new CBlocks<IMyBatteryBlock>();
}

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplays("Статус");
  initGroups();
  return "Отображение статуса";
}

public void main(string argument, UpdateType updateSource)
{
  int i = 0;
  lcd.showStatus<IMyShipDrill>(drills, i++);
  lcd.showStatus<IMyCargoContainer>(storage, i++);
  lcd.showStatus<IMyShipConnector>(connectors, i++);
  lcd.showStatus<IMyThrust>(thrusters, i++);
  lcd.showStatus<IMyGyro>(gyroscopes, i++);
  lcd.showStatus<IMyBatteryBlock>(battaryes, i++);
}
