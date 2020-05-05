// #include classes/blocks_group.cs

public CBlockGroup<IMyShipMergeBlock> weldersMergers;
public CBlockGroup<IMyPistonBase> weldersMergersPistons;
public CBlockGroup<IMyShipMergeBlock> supportMergers;
public CBlockGroup<IMyPistonBase> supportMergersPistons;
public CBlockGroup<IMyShipConnector> mainConnectors;
public CBlockGroup<IMyPistonBase> mainPistons;
public CBlockGroup<IMyShipWelder> welders;
public CBlockGroup<IMyProjector> projectors;

public void initGroups()
{
  weldersMergers = new CBlockGroup<IMyShipMergeBlock>("[Паук] Соединители верхней опоры", "СВО");
  weldersMergersPistons = new CBlockGroup<IMyPistonBase>("[Паук] Поршни соединителей верхней опоры", "Поршни СВО");
  supportMergers = new CBlockGroup<IMyShipMergeBlock>("[Паук] Соединители нижней опоры", "СНО");
  supportMergersPistons = new CBlockGroup<IMyPistonBase>("[Паук] Поршни соединителей нижней опоры", "Поршни СНО");
  mainPistons = new CBlockGroup<IMyPistonBase>("[Паук] Поршни хода", "Поршни хода");
  welders = new CBlockGroup<IMyShipWelder>("[Паук] Сварщики", "Сварщики");
  projectors = new CBlockGroup<IMyProjector>("[Паук] Проекторы", "Проекторы");
}
