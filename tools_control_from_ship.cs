// #include classes/main.cs
// #include classes/display.cs
// #include classes/state_machine.cs
// #include classes/blocks/piston.cs
// #include classes/blocks/merger.cs
// #include classes/blocks/connector.cs
// #include classes/blocks/ship_tool.cs
// #include classes/blocks/sensor.cs
// #include classes/blocks/lamp.cs
// #include classes/blocks/piston.cs
// #include helpers/bool.cs

// #include parts/tools_control/tools_control.cs

CPiston    toolExtender;
CConnector toolConnector;
CConnector parkingConnector;
CConnector shipConnector;
CMerger    shipMerger;
CMerger    toolMerger;

CStateMachine connectStates;
CStateMachine parkStates;

CDisplay statusLcd;

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  shipMerger    = new CMerger   (new CBlocksNamed<IMyShipMergeBlock>("Корабельный"));
  shipConnector = new CConnector(new CBlocksNamed<IMyShipConnector >("Корабельный"));
  toolExtender  = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Инструмент" ));

  statusLcd = new CDisplay();
  statusLcd.addDisplays("Лог");

  connectStates = new CStateMachine(statusLcd);
  connectStates.addState("Слияние структур", merge);
  connectStates.addState("Соединение коннекторов", connect);
  connectStates.addState("Переименование", renameGrid);
  connectStates.addState("Инициализация инструмента", connectedToolsInit);
  connectStates.addState("Отключение парковочного коннектора", unpark);
  connectStates.addState("Ожидание отхода", unparkBack);
  connectStates.addState("Отключение инструмента", toolsOff);

  parkStates = new CStateMachine(statusLcd);
  parkStates.addState("Отключение инструмента", toolsOff);
  parkStates.addState("Соединение парковочного коннектора", park);
  parkStates.addState("Отключение корабля", disconnect);
  parkStates.addState("Разделение структур", unmerge);
  parkStates.addState("Переименование", renameGrid);
  parkStates.addState("Ожидание отхода", parkBack);
  parkStates.addState("Отмена инициализации", connectedToolsDestroy);

  connectedToolsInit(null);
  return "Контроль инструментального модуля";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument.Length == 0)
  {
    if(connectStates.active()) { connectStates.step(); }
    if(parkStates   .active()) { parkStates   .step(); }
  }
  else if(!toolsArgumentsParse(argument))
  {
    switch(argument)
    {
      case "catch"       : { catchTool(); } break;
      case "expand"      : { if (toolsAvailable()) { toolExtender.expandRelative (1f,2f) ; } } break;
      case "retract"     : { if (toolsAvailable()) { toolExtender.retractRelative(1f,2f) ; } } break;
      case "expand_full" : { if (toolsAvailable()) { toolExtender.expand         (10f,2f); } } break;
      case "retract_full": { if (toolsAvailable()) { toolExtender.retract        (0f ,2f); } } break;
    }
  }
}


public bool renameGrid(object data) { Me.CubeGrid.CustomName = structureName; return Me.CubeGrid.CustomName == structureName; }

public bool connect (object data) { return  shipConnector   .connect(); }
public bool park    (object data) { return  parkingConnector.connect(); }
public bool parkBack(object data) { return !shipConnector   .connectable(); }
public bool merge   (object data) { return  shipMerger      .connect(); }

public bool disconnect(object data) { return  shipConnector   .disconnect (); }
public bool unpark    (object data) { return  parkingConnector.disconnect (); }
public bool unparkBack(object data) { return !parkingConnector.connectable(); }
public bool unmerge   (object data) { return  shipMerger      .disconnect (); }

public bool connectedToolsInit(object data)
{
  toolConnector    = new CConnector(new CBlocksNamed<IMyShipConnector >("Инструментальный"));
  parkingConnector = new CConnector(new CBlocksNamed<IMyShipConnector >("Парковка"));
  toolMerger       = new CMerger   (new CBlocksNamed<IMyShipMergeBlock>("Инструментальный"));
  return toolsInit(data) &&
         toolConnector    != null && !toolConnector   .empty() &&
         parkingConnector != null && !parkingConnector.empty() &&
         toolMerger       != null && !toolMerger      .empty();
}

public bool connectedToolsDestroy(object data)
{
  toolConnector = null;
  parkingConnector = null;
  toolMerger = null;
  return !toolsDestroy(data);
}

public bool toolsOn(object data)
{
  bool result = on();
  statusLcd.echo($"Статус: {boolToString(result)}");
  return result;
}

public bool toolsOff(object data)
{
  bool result = off();
  statusLcd.echo($"Статус: {boolToString(result)}");
  return result;
}

public void catchTool()
{
  statusLcd.echo("Стыковка инструмента");
  if(!toolsAvailable())
  {
    statusLcd.echo("Запрос стыковки с инструментом");
    if(shipConnector.connectable())
    {
      connectStates.start(true);
    } else { statusLcd.echo($"Корабельный коннектор не в состоянии ожидания ({boolToString(shipConnector.connectable())})"); }
  }
  else
  {
    statusLcd.echo("Запрос расстыковки с инструментом");
    if (parkingConnector.connectable())
    {
      parkStates.start(true);
    } else { statusLcd.echo($"Парковочный коннектор не в состоянии ожидания ({boolToString(parkingConnector.connectable())})"); }
  }
}