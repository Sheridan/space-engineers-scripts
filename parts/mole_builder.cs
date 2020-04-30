// #include classes/blocks_group.cs

public CBlockGroup<IMyShipMergeBlock> weldersMergers;
public CBlockGroup<IMyPistonBase> weldersMergersPistons;
public CBlockGroup<IMyShipMergeBlock> supportMergers;
public CBlockGroup<IMyPistonBase> supportMergersPistons;
public CBlockGroup<IMyShipMergeBlock> logisticMergers;
public CBlockGroup<IMyPistonBase> logisticPistons;
public CBlockGroup<IMyShipConnector> logisticConnectors;
public CBlockGroup<IMyShipConnector> mainConnectors;
public CBlockGroup<IMyPistonBase> mainPistons;
public CBlockGroup<IMyShipWelder> welders;
public CBlockGroup<IMyProjector> projectors;

public void initGroups()
{
  weldersMergers = new CBlockGroup<IMyShipMergeBlock>("[Крот] Соединители нижних коннекторов", "НС");
  weldersMergersPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни нижних коннекторов", "Поршни НС");
  supportMergers = new CBlockGroup<IMyShipMergeBlock>("[Крот] Соединители верхних коннекторов", "ВС");
  supportMergersPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни верхних коннекторов", "Поршни ВС");
  logisticMergers = new CBlockGroup<IMyShipMergeBlock>("[Крот] Соединители логистики", "ЛС");
  logisticPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни коннекторов", "Поршни ЛС");
  mainPistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни хода сварки", "Поршни хода");
  logisticConnectors = new CBlockGroup<IMyShipConnector>("[Крот] Коннекторы логистики", "Коннекторы");
  mainConnectors = new CBlockGroup<IMyShipConnector>("[Крот] Основные коннекторы ресурсов", "Баз. коннекторы");
  welders = new CBlockGroup<IMyShipWelder>("[Крот] Сварщики", "Сварщики");
  projectors = new CBlockGroup<IMyProjector>("[Крот] Проекторы", "Проекторы");
}
