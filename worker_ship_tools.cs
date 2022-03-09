// #include classes/main.cs
// #include classes/display.cs
// #include classes/state_machine.cs
// #include classes/blocks.cs
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
  frontShipMerger = new CMerger(new CBlocks<IMyShipMergeBlock>("Фронтальный"));
  bottomShipMerger = new CMerger(new CBlocks<IMyShipMergeBlock>("Нижний"));

  frontShipConnector = new CConnector(new CBlocks<IMyShipConnector>("Фронтальный"));
  bottomShipConnector = new CConnector(new CBlocks<IMyShipConnector>("Нижний"));

  statusLcd = new CDisplay();
  statusLcd.addDisplay("[Универсал] Дисплей Лог 1", 0, 0);
  statusLcd.addDisplay("[Универсал] Дисплей Лог 0", 1, 0);
  statusLcd.addDisplay("[Универсал] Дисплей Лог 2", 2, 0);

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

  toolsInit();
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
  //  else if (argument == "on" ) { if (toolsConnected) { toolsOn (); } }
  //  else if (argument == "off") { if (toolsConnected) { toolsOff(); } }
    else if (argument == "onoff")
    {
      if (toolsConnected) { if (!toolsActive) { toolsOn(); } else { toolsOff(); } }
    }
    else if (argument == "expand" ) { if (toolsConnected) { toolExtender.expandRelative (1,1); } }
    else if (argument == "retract") { if (toolsConnected) { toolExtender.retractRelative(1,1); } }
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

public bool renameGrid() { Me.CubeGrid.CustomName = structureName; return Me.CubeGrid.CustomName == structureName; }

public bool connect () { return  activeShipConnector.connect(); }
public bool park    () { return  parkingConnector   .connect(); }
public bool parkBack() { return !activeShipConnector.connectable(); }
public bool merge   () { return  activeShipMerger   .connect(); }

public bool disconnect() { return  activeShipConnector.disconnect (); }
public bool unpark    () { return  parkingConnector   .disconnect (); }
public bool unparkBack() { return !parkingConnector   .connectable(); }
public bool unmerge   () { return  activeShipMerger   .disconnect (); }

public bool toolsInit()
{
  tools = new CShipTool(new CBlocks<IMyShipToolBase>());
  // toolLcd = new CDisplay();
  // toolLcd.addDisplay("[Универсал] Дисплей Статус 0", 0, 0);
  toolConnector    = new CConnector(new CBlocks<IMyShipConnector >("Инструментальный"));
  parkingConnector = new CConnector(new CBlocks<IMyShipConnector >("Парковка"));
  toolMerger       = new CMerger   (new CBlocks<IMyShipMergeBlock>("Инструментальный"));
  safetySensor     = new CSensor   (new CBlocks<IMySensorBlock   >("Безопасность"));
  safetyLamp       = new CLamp     (new CBlocks<IMyLightingBlock >("Безопасность"));
  toolExtender     = new CPiston   (new CBlocks<IMyPistonBase    >("Удлиннитель"));
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

public bool toolsOn()
{
  safetyLamp.enable();
  safetySensor.enable();
  toolsActive = safetyLamp.enabled() && safetySensor.enabled() && tools.on();
  statusLcd.echo($"Статус: {boolToString(toolsActive)}");
  return toolsActive;
}

public bool toolsOff()
{
  safetyLamp.disable();
  safetySensor.disable();
  toolsActive = !(!safetyLamp.enabled() && !safetySensor.enabled() && tools.off());
  statusLcd.echo($"Статус: {boolToString(toolsActive)}");
  return !toolsActive;
}
