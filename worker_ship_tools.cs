// #include classes/main.cs
// #include classes/display.cs
// #include classes/state_machine.cs
// #include classes/blocks/base/blocks_named.cs
// #include classes/blocks/base/blocks.cs
// #include classes/blocks/piston.cs
// #include classes/blocks/merger.cs
// #include classes/blocks/connector.cs
// #include classes/blocks/ship_tool.cs
// #include classes/blocks/sensor.cs
// #include classes/blocks/lamp.cs
// #include classes/blocks/piston.cs
// #include helpers/bool.cs

CShipTool tools;

CConnector frontShipConnector;
CConnector bottomShipConnector;
CConnector activeShipConnector;
CConnector toolConnector;
CConnector parkingConnector;

CMerger frontShipMerger;
CMerger bottomShipMerger;
CMerger activeShipMerger;
CMerger toolMerger;

CPiston toolExtender;

CStateMachine connectStates;
CStateMachine parkStates;

CDisplay statusLcd;
// CDisplay toolLcd;

CSensor safetySensor;
CLamp safetyLamp;

private bool toolsConnected;
private bool toolsActive;

public string program()
{
  frontShipMerger = new CMerger(new CBlocksNamed<IMyShipMergeBlock>("Фронтальный"));
  bottomShipMerger = new CMerger(new CBlocksNamed<IMyShipMergeBlock>("Нижний"));

  frontShipConnector = new CConnector(new CBlocksNamed<IMyShipConnector>("Фронтальный"));
  bottomShipConnector = new CConnector(new CBlocksNamed<IMyShipConnector>("Нижний"));

  statusLcd = new CDisplay();
  statusLcd.addDisplays("Лог");

  connectStates = new CStateMachine(statusLcd);
  connectStates.addState("Слияние структур", merge);
  connectStates.addState("Соединение коннекторов", connect);
  connectStates.addState("Переименование", renameGrid);
  connectStates.addState("Инициализация инструмента", toolsInit);
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
  parkStates.addState("Отмена инициализации", toolsInit);

  toolsInit(null);
  searchActiveShipConnector();
  return "Управление инструментом";
}

public void main(string argument, UpdateType updateSource)
{
  if (argument.Length > 0)
  {
    if(argument == "catch")
    {
      statusLcd.echo("Стыковка инструмента");
      if(!toolsConnected)
      {
        statusLcd.echo("Запрос стыковки с инструментом");
        searchActiveShipConnector();
        if(activeShipConnector != null && activeShipConnector.connectable())
        {
          connectStates.start(true);
        } else { statusLcd.echo($"Корабельный коннектор не в состоянии ожидания"); }
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
    else if (argument == "on" ) { if (toolsConnected) { toolsOn (null); } }
    else if (argument == "off") { if (toolsConnected) { toolsOff(null); } }
    else if (argument == "onoff")
    {
      if (toolsConnected) { if (!toolsActive) { toolsOn(null); } else { toolsOff(null); } }
    }
    else if (argument == "expand" )      { if (toolsConnected) { toolExtender.expandRelative (1f,2f); } }
    else if (argument == "retract")      { if (toolsConnected) { toolExtender.retractRelative(1f,2f); } }
    else if (argument == "expand_full" ) { if (toolsConnected) { toolExtender.expand (10f,2f); } }
    else if (argument == "retract_full") { if (toolsConnected) { toolExtender.retract(0f ,2f); } }
  }
  else
  {
    if(connectStates.active()) { connectStates.step(); }
    if(parkStates   .active()) { parkStates   .step(); }
  }
}

void searchActiveShipConnector()
{
  string activeConnectorPos = "";
  if(toolsConnected)
  {
    if (frontShipConnector.connected())
    {
      activeShipConnector     = frontShipConnector;
      activeShipMerger        = frontShipMerger;
      activeConnectorPos      = "Фронтальный";
    }
    else if (bottomShipConnector.connected())
    {
      activeShipConnector     = bottomShipConnector;
      activeShipMerger        = bottomShipMerger;
      activeConnectorPos      = "Нижний";
    }
  }
  else
  {
    if (frontShipConnector.connectable())
    {
      activeShipConnector     = frontShipConnector;
      activeShipMerger        = frontShipMerger;
      activeConnectorPos      = "Фронтальный";
    }
    else if (bottomShipConnector.connectable())
    {
      activeShipConnector     = bottomShipConnector;
      activeShipMerger        = bottomShipMerger;
      activeConnectorPos      = "Нижний";
    }
  }
  if(activeShipConnector != null)
  {
    statusLcd.echo($"Активный коннектор: {activeConnectorPos} ({activeShipConnector.count()}:{activeShipMerger.count()}), ({boolToString(activeShipConnector.connected())},{boolToString(activeShipMerger.connected())})");
  }
  else
  {
    statusLcd.echo("Инструменты не подключены и не найдены в зоне доступа");
  }
}

public bool renameGrid(object data) { Me.CubeGrid.CustomName = structureName; return Me.CubeGrid.CustomName == structureName; }

public bool connect (object data) { return  activeShipConnector.connect(); }
public bool park    (object data) { return  parkingConnector   .connect(); }
public bool parkBack(object data) { return !activeShipConnector.connectable(); }
public bool merge   (object data) { return  activeShipMerger   .connect(); }

public bool disconnect(object data) { return  activeShipConnector.disconnect (); }
public bool unpark    (object data) { return  parkingConnector   .disconnect (); }
public bool unparkBack(object data) { return !parkingConnector   .connectable(); }
public bool unmerge   (object data) { return  activeShipMerger   .disconnect (); }

public bool toolsInit(object data)
{
  tools = new CShipTool(new CBlocks<IMyShipToolBase>());
  // toolLcd = new CDisplay();
  // toolLcd.addDisplay("[Универсал] Дисплей Статус 0", 0, 0);
  toolConnector    = new CConnector(new CBlocksNamed<IMyShipConnector >("Инструментальный"));
  parkingConnector = new CConnector(new CBlocksNamed<IMyShipConnector >("Парковка"));
  toolMerger       = new CMerger   (new CBlocksNamed<IMyShipMergeBlock>("Инструментальный"));
  safetySensor     = new CSensor   (new CBlocksNamed<IMySensorBlock   >("Безопасность"));
  safetyLamp       = new CLamp     (new CBlocksNamed<IMyLightingBlock >("Безопасность"));
  toolExtender     = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Удлиннитель"));
  toolsConnected =  toolMerger       != null && !toolMerger.empty() &&
                    parkingConnector != null && !parkingConnector.empty() &&
                    toolConnector    != null && !toolConnector.empty() &&
                    toolExtender     != null && !toolExtender.empty() &&
                    tools != null && !tools.empty();
  showStatus();
  // if(connectStates.active()) { return  toolsConnected; }
  // if(parkStates   .active()) { return !toolsConnected; }
  return (connectStates.active() && toolsConnected) || (parkStates.active() && !toolsConnected);
}

public void showStatus()
{
  statusLcd.echo($"Статус фронтального коннектора ({frontShipConnector.count()}): {boolToString(frontShipConnector.connected())}");
  statusLcd.echo($"Статус фронтального мержера ({frontShipMerger.count()}): {boolToString(frontShipMerger.connected())}");
  statusLcd.echo($"Статус нижнего коннектора ({bottomShipConnector.count()}): {boolToString(bottomShipConnector.connected())}");
  statusLcd.echo($"Статус нижнего мержера ({bottomShipMerger.count()}): {boolToString(bottomShipMerger.connected())}");
  statusLcd.echo($"Статус подключения: {boolToString(toolsConnected)}");
  if(toolsConnected)
  {
    statusLcd.echo($"Статус коннектора инструментов ({toolConnector.count()}): {boolToString(toolConnector.connected())}");
    statusLcd.echo($"Статус коннектора парковки ({parkingConnector.count()}): {boolToString(parkingConnector.connected())}");
    statusLcd.echo($"Статус мержера ({toolMerger.count()}): {boolToString(toolMerger.connected())}");
    statusLcd.echo($"Статус лампы ({safetyLamp.count()}): {boolToString(toolMerger.enabled())}");
    statusLcd.echo($"Статус сенсора ({safetySensor.count()}): {boolToString(safetySensor.enabled())}");
    statusLcd.echo($"Статус инструментов ({tools.count()}): {boolToString(tools.enabled())}");
  }
}

public bool toolsOn(object data)
{
  safetyLamp.enable();
  safetySensor.enable();
  toolsActive = safetyLamp.enabled() && safetySensor.enabled() && tools.on();
  statusLcd.echo($"Статус: {boolToString(toolsActive)}");
  return toolsActive;
}

public bool toolsOff(object data)
{
  safetyLamp.disable();
  safetySensor.disable();
  toolsActive = !(!safetyLamp.enabled() && !safetySensor.enabled() && tools.off());
  statusLcd.echo($"Статус: {boolToString(toolsActive)}");
  return !toolsActive;
}
