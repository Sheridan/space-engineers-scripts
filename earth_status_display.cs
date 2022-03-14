// #include classes/main.cs
// #include classes/blockstatus_display.cs
// #include classes/blocks/base/blocks_typed.cs
// #include classes/blocks/base/blocks.cs
// #include classes/blocks/base/blocks_named.cs

CBlockStatusDisplay lcd;

public CBlocksTyped<IMyPowerProducer> windTurbines;
public CBlocks<IMySolarPanel> solarPanels;
public CBlocks<IMyBatteryBlock> battaryes;
public CBlocksTyped<IMyPowerProducer> h2Engines;
public CBlocks<IMyShipConnector> connectors;
public CBlocks<IMyRefinery> refineryes;
public CBlocks<IMyAssembler> assemblers;
public CBlocks<IMyGasGenerator> gasGenerators;
public CBlocksNamed<IMyCargoContainer> storageOre;
public CBlocksNamed<IMyCargoContainer> storageIce;
public CBlocksNamed<IMyCargoContainer> storageIngots;
public CBlocksNamed<IMyCargoContainer> storageComponents;
public CBlocksNamed<IMyGasTank> o2tanks;
public CBlocksTyped<IMyGasTank> h2tanks;
// public CBlockGroup<IMyPistonBase> weldersMergersPistons;
// public CBlockGroup<IMyShipMergeBlock> supportMergers;
// public CBlockGroup<IMyPistonBase> supportMergersPistons;
// public CBlockGroup<IMyShipMergeBlock> logisticMergers;
// public CBlockGroup<IMyPistonBase> logisticPistons;
// public CBlockGroup<IMyShipConnector> logisticConnectors;
// public CBlockGroup<IMyShipConnector> mainConnectors;
// public CBlockGroup<IMyPistonBase> mainPistons;
// public CBlockGroup<IMyShipWelder> welders;
// public CBlockGroup<IMyProjector> projectors;

public void initGroups()
{
  h2Engines    = new CBlocksTyped<IMyPowerProducer>("HydrogenEngine");
  windTurbines = new CBlocksTyped<IMyPowerProducer>("WindTurbine");
  solarPanels  = new CBlocks<IMySolarPanel>();
  battaryes    = new CBlocks<IMyBatteryBlock>();

  connectors    = new CBlocks<IMyShipConnector>();
  refineryes    = new CBlocks<IMyRefinery>();
  assemblers    = new CBlocks<IMyAssembler>();
  gasGenerators = new CBlocks<IMyGasGenerator>();

  storageOre        = new CBlocksNamed<IMyCargoContainer>("К Руда");
  storageIngots     = new CBlocksNamed<IMyCargoContainer>("К Слитки");
  storageIce        = new CBlocksNamed<IMyCargoContainer>("К Лёд");
  storageComponents = new CBlocksNamed<IMyCargoContainer>("К Компоненты");

  o2tanks = new CBlocksNamed<IMyGasTank>("O2");
  h2tanks = new CBlocksTyped<IMyGasTank>("HydrogenTank");
  // weldersMergers = new CBlockGroup<IMyShipMergeBlock>("[Крот] Соединители нижних коннекторов", "НС");
  // weldersMergersPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни нижних коннекторов", "Поршни НС");
  // supportMergers = new CBlockGroup<IMyShipMergeBlock>("[Крот] Соединители верхних коннекторов", "ВС");
  // supportMergersPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни верхних коннекторов", "Поршни ВС");
  // logisticMergers = new CBlockGroup<IMyShipMergeBlock>("[Крот] Соединители логистики", "ЛС");
  // logisticPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни коннекторов", "Поршни ЛС");
  // mainPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни хода сварки", "Поршни хода");
  // logisticConnectors = new CBlockGroup<IMyShipConnector>("[Крот] Коннекторы логистики", "Коннекторы");
  // mainConnectors = new CBlockGroup<IMyShipConnector>("[Крот] Основные коннекторы ресурсов", "Баз. коннекторы");
  // welders = new CBlockGroup<IMyShipWelder>("[Крот] Сварщики", "Сварщики");
  // projectors = new CBlockGroup<IMyProjector>("[Крот] Проекторы", "Проекторы");
  debug("Done groups");
}


public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplays("Статус");
  debug("Done displays");
  initGroups();
  return "Отображение статуса базы";
}

public void main(string argument, UpdateType updateSource)
{
  int i = 0;
  // debug("!");
  if (argument == "restart") { initGroups(); }
  lcd.showStatus<IMyPowerProducer>(windTurbines, i++);
  lcd.showStatus<IMySolarPanel>(solarPanels, i++);
  lcd.showStatus<IMyBatteryBlock>(battaryes, i++);
  lcd.showStatus<IMyPowerProducer>(h2Engines, i++);
  lcd.showStatus<IMyShipConnector>(connectors, i++);
  lcd.showStatus<IMyRefinery>(refineryes, i++);
  lcd.showStatus<IMyAssembler>(assemblers, i++);
  lcd.showStatus<IMyGasGenerator>(gasGenerators, i++);
  lcd.showStatus<IMyCargoContainer>(storageOre, i++);
  lcd.showStatus<IMyCargoContainer>(storageIce, i++);
  lcd.showStatus<IMyCargoContainer>(storageIngots, i++);
  lcd.showStatus<IMyCargoContainer>(storageComponents, i++);
  lcd.showStatus<IMyGasTank>(o2tanks, i++);
  lcd.showStatus<IMyGasTank>(h2tanks, i++);
  // lcd.showStatus<IMyShipMergeBlock>(supportMergers, 2);
  // lcd.showStatus<IMyPistonBase>(supportMergersPistons, 3);
  // lcd.showStatus<IMyShipMergeBlock>(logisticMergers, 4);
  // lcd.showStatus<IMyPistonBase>(logisticPistons, 5);
  // lcd.showStatus<IMyPistonBase>(mainPistons, 6);
  // lcd.showStatus<IMyShipConnector>(logisticConnectors, 7);
  // lcd.showStatus<IMyShipConnector>(mainConnectors, 8);
  // lcd.showStatus<IMyShipWelder>(welders, 9);
  // lcd.showStatus<IMyProjector>(projectors, 10);
  // lcd.echo($"Line {i}");
  // i++;
}
