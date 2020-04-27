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
  // lcd.echo($"Line {i}");
  // i++;
}

// const string stateLCDName = "[Крот] Дисплей состояния";
// const string mainPistonsGroupName = ;
// const float mainPistonsInStack = 3f;
// const float mainPistonsExpandVelocity = 1f;
// const float mainPistonsRetractVelocity = 1f;
// const float mainPistonsMinLength = blockHeight;
// const float mainPistonsMaxLength = structureHeight + blockHeight;
// const float connPistonsExpandVelocity = 1f;
// const float connPistonsRetractVelocity = 2f;
// const string weldersMergersGroupName = ;
// const string weldersMergersPistonsGroupName = ;
// const string supportMergersGroupName = ;
// const string supportMergersPistonsGroupName =
// const string logisticPistonsGroupName = ;
// const string logisticMergersGroupName =
// const string logisticConnectorsGroupName = ;
// const string proectorsGroupName = ;
// const string weldersGroupName = ;
// const string componentsContainerName = "[Крот] БК";
// const string componentsSourceContainersGroupName = "[Земля] БК Компоненты";
// const string mainConnectorsGroupName = ;
// const string soundBlockName = "[Крот] Динамик";
