// #include classes/main.cs
// #include classes/blockstatus_display.cs
// #include classes/blocks_typed.cs
// #include classes/blocks.cs
// #include classes/blocks_group.cs

CBlockStatusDisplay lcd;

public CBlocksTyped<IMyPowerProducer> windTurbines;
public CBlocksTyped<IMyPowerProducer> h2Engines;
public CBlocks<IMyShipConnector> connectors;
public CBlocks<IMyRefinery> refineryes;
public CBlocks<IMyAssembler> assemblers;
public CBlocks<IMyGasGenerator> gasGenerators;
public CBlockGroup<IMyCargoContainer> storageOre;
public CBlockGroup<IMyCargoContainer> storageIngots;
public CBlockGroup<IMyCargoContainer> storageComponents;
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
  h2Engines = new CBlocksTyped<IMyPowerProducer>("HydrogenEngine");
  windTurbines = new CBlocksTyped<IMyPowerProducer>("WindTurbine");
  connectors = new CBlocks<IMyShipConnector>();
  refineryes = new CBlocks<IMyRefinery>();
  assemblers = new CBlocks<IMyAssembler>();
  gasGenerators = new CBlocks<IMyGasGenerator>();

  storageOre = new CBlockGroup<IMyCargoContainer>("[Земля] БК Руда", "Руда");
  storageIngots = new CBlockGroup<IMyCargoContainer>("[Земля] БК Слитки", "Слитки");
  storageComponents = new CBlockGroup<IMyCargoContainer>("[Земля] БК Компоненты", "Компоненты");
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
}


public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CBlockStatusDisplay();
  lcd.addDisplay("[Земля] Дисплей статуса 0", 0, 0);
  lcd.addDisplay("[Земля] Дисплей статуса 1", 1, 0);
  initGroups();
  return "Отображение статуса базы";
}

public void main(string argument, UpdateType updateSource)
{
  lcd.showStatus<IMyPowerProducer>(windTurbines, 0);
  lcd.showStatus<IMyPowerProducer>(h2Engines, 1);
  lcd.showStatus<IMyShipConnector>(connectors, 2);
  lcd.showStatus<IMyRefinery>(refineryes, 3);
  lcd.showStatus<IMyAssembler>(assemblers, 4);
  lcd.showStatus<IMyGasGenerator>(gasGenerators, 5);
  lcd.showStatus<IMyCargoContainer>(storageOre, 6);
  lcd.showStatus<IMyCargoContainer>(storageIngots, 7);
  lcd.showStatus<IMyCargoContainer>(storageComponents, 8);
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
