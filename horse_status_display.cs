// #include classes/main.cs
// #include classes/blockstatus_display.cs
// #include classes/blocks_typed.cs
// #include classes/blocks.cs

CBlockStatusDisplay lcd;

public CBlocks<IMyShipConnector> connectors;
public CBlocks<IMyGasGenerator> gasGenerators;
public CBlocks<IMyThrust> thrusters;
public CBlocks<IMyCargoContainer> storage;
public CBlocks<IMyGyro> gyroscopes;
public CBlocks<IMyLargeGatlingTurret> turrets;
public CBlocks<IMyOxygenTank> o2tanks;
public CBlocksTyped<IMyGasTank> h2tanks;
public CBlocks<IMyBatteryBlock> battaryes;

public void initGroups()
{
  connectors = new CBlocks<IMyShipConnector>();
  gasGenerators = new CBlocks<IMyGasGenerator>();
  storage = new CBlocks<IMyCargoContainer>();
  thrusters = new CBlocks<IMyThrust>();
  gyroscopes = new CBlocks<IMyGyro>();
  turrets = new CBlocks<IMyLargeGatlingTurret>();
  o2tanks = new CBlocks<IMyOxygenTank>();
  h2tanks = new CBlocksTyped<IMyGasTank>("HydrogenTank");
  battaryes = new CBlocks<IMyBatteryBlock>();
}


public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplay("[Конь] Дисплей статуса 0", 0, 0);
  lcd.addDisplay("[Конь] Дисплей статуса 1", 1, 0);
  initGroups();
  return "Отображение статуса";
}

public void main(string argument, UpdateType updateSource)
{
  lcd.showStatus<IMyShipConnector>(connectors, 0);
  lcd.showStatus<IMyGasGenerator>(gasGenerators, 1);
  lcd.showStatus<IMyCargoContainer>(storage, 2);
  lcd.showStatus<IMyThrust>(thrusters, 3);
  lcd.showStatus<IMyGyro>(gyroscopes, 4);
  lcd.showStatus<IMyLargeGatlingTurret>(turrets, 5);
  lcd.showStatus<IMyOxygenTank>(o2tanks, 6);
  lcd.showStatus<IMyGasTank>(h2tanks, 7);
  lcd.showStatus<IMyBatteryBlock>(battaryes, 8);
}
