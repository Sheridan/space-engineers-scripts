// #include classes/main.cs
// #include classes/blocks/base/blocks_named.cs
// #include classes/blocks/rotor.cs
// #include classes/blocks/piston.cs
// #include classes/blocks/merger.cs
// #include classes/blocks/connector.cs
// #include classes/blocks/ship_tool.cs
// #include classes/blocks/projector.cs
// #include classes/state_machine.cs
// #include classes/display.cs
// #include helpers/bool.cs
// #include classes/blocks/speaker.cs

float drillRotorsRPM = 0.5f;

float wallHeight = 4f * 2.5f;
float drillSteps = 8f;
float drillPistonsStep;
float welderSteps = 4f;
float welderPistonsStep;
float pistonsSpeed = 1f;
float lastRotorsAngle = 0f;
float stepRotorsAngle = 190f;
float continuosRotorAngle = 0f;

int lastAngleFactor = 1;

CDisplay logLcd;
CDisplay statusLcd;
CRotor drillRotors0;
CRotor drillRotors45;
CRotor beltRotor;
CPiston drillPistons;
CPiston welderPistons;
CPiston platformPistons;
CShipTool drills;
CShipTool welders;
CShipTool beltGrinder;
CShipTool beltWelder;
CMerger topMergers;
CMerger bottomMergers;
CPiston topPistons;
CPiston bottomPistons;
CPiston connectorPiston;
CProjector wallProjector;
CProjector beltProjector;
CConnector platformConnector;
CSpeaker speaker;

CStateMachine states;

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  drillPistonsStep  = wallHeight / drillSteps;
  welderPistonsStep = wallHeight / welderSteps;

  logLcd = new CDisplay();
  logLcd.addDisplay($"[{structureName}] Дисплей Лог 1", 0, 0);
  logLcd.addDisplay($"[{structureName}] Дисплей Лог 0", 1, 0);

  statusLcd = new CDisplay();
  statusLcd.addDisplay($"[{structureName}] Дисплей Статус 0", 0, 0);

  drillRotors0      = new CRotor    (new CBlocksNamed<IMyMotorStator   >("Инструмент (0)"));
  drillRotors45     = new CRotor    (new CBlocksNamed<IMyMotorStator   >("Инструмент (45)"));
  drillPistons      = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Бур"));
  welderPistons     = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Сварщик"));
  platformPistons   = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Подвес"));
  drills            = new CShipTool (new CBlocksNamed<IMyShipToolBase  >("Бур"));
  welders           = new CShipTool (new CBlocksNamed<IMyShipToolBase  >("Сварщик Стена"));
  topMergers        = new CMerger   (new CBlocksNamed<IMyShipMergeBlock>("Верхний"));
  bottomMergers     = new CMerger   (new CBlocksNamed<IMyShipMergeBlock>("Нижний"));
  topPistons        = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Верхний"));
  bottomPistons     = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Нижний"));
  wallProjector     = new CProjector(new CBlocksNamed<IMyProjector     >("Стена"));
  beltProjector     = new CProjector(new CBlocksNamed<IMyProjector     >("Конвейер"));
  platformConnector = new CConnector(new CBlocksNamed<IMyShipConnector >("Конвейер"));
  connectorPiston   = new CPiston   (new CBlocksNamed<IMyPistonBase    >("Конвейер"));
  speaker           = new CSpeaker  (new CBlocksNamed<IMySoundBlock    >("Динамик"));
  beltRotor         = new CRotor    (new CBlocksNamed<IMyMotorStator   >("Конвейер"));
  beltGrinder       = new CShipTool (new CBlocksNamed<IMyShipToolBase  >("Резак Конвейер"));;
  beltWelder        = new CShipTool (new CBlocksNamed<IMyShipToolBase  >("Сварщик Конвейер"));;

  states = new CStateMachine(logLcd, speaker);
  states.addState("Подготовка к запуску", prepareStart);
  states.addState("Запуск вращения буров", startDrillRotors);
  states.addState("Запуск буров", startDrill);
  for(int i = 1; i<=drillSteps; i++)
  {
    states.addState($"Опускание буров (шаг {i})", stepDrillPiston, i*drillPistonsStep);
    states.addState($"Получение текущего угла роторов (шаг {i})", prepareRotorsAngleCalc);
    states.addState($"Ожидание поворота роторов (шаг {i})", waitRotorsTurn);
  }
  states.addState("Остановка буров", stopDrill);
  states.addState("Остановка вращения буров", stopDrillRotors);
  states.addState("Поднятие поршней буров", retractDrillPiston);
  states.addState("Установка буров в начальную позицию", toZero);

  states.addState("Запуск сварщиков", startWelder);
  states.addState("Запуск проектора стен", tunnelProjectorOn);
  for(int i = 1; i<=welderSteps; i++)
  {
    states.addState($"Опускание сварщиков (шаг {i})", stepWelderPiston, i*welderPistonsStep);
    states.addState($"Ожидание сварщиков (шаг {i})", waitWallWelders, 600+i*120);
  }
  states.addState("Остановка сварщиков", stopWelder);
  states.addState("Остановка проектора стен", tunnelProjectorOff);

  states.addState("Расстыковка нижних соединителей", disconnectBottomMergers);
  states.addState("Сворачивание нижних поршней", retractBottomPistons);
  states.addState("Расстыковка коннектора", disconnectPlatformConnector);
  states.addState("Сворачивание поршня коннектора", retractConnectorPiston);

  states.addState("Включение проектора трубы", beltProjectorOn);
  states.addState("Запуск срезания коннектора", startBeltGrinder);
  states.addState("Ожидание срезания коннектора", waitBeltGrinder);
  states.addState("Остановка срезания коннектора", stopBeltGrinder);
  states.addState("Смена инструмента на сварщик", rotateToWelder);
  states.addState("Ожидание смены инструмента", waitToolRotateToWelder);
  states.addState("Остановка ротора", stopToolRotate);
  states.addState("Запуск сварки трубы", startBeltWelder);
  for(int i = 1; i<=welderSteps; i++)
  {
    states.addState($"Спуск платформы (шаг {i})", platformDown, i*welderPistonsStep);
    states.addState($"Ожидание сварщика трубы (шаг {i})", waitBeltWelder, i+1);
  }
  states.addState("Остановка сварки трубы", stopBeltWelder);
  states.addState("Отключение проектора трубы", beltProjectorOff);
  states.addState("Смена инструмента на резак", rotateToGrinder);
  states.addState("Ожидание смены инструмента", waitToolRotateToGrinder);
  states.addState("Остановка ротора", stopToolRotate);

  states.addState("Разворачивание поршня коннектора", expandConnectorPiston);
  states.addState("Стыковка коннектора", connectPlatformConnector);
  states.addState("Включение нижних соединителей", connectBottomMergers);
  states.addState("Разворачивание нижних поршней", expandBottomPistons);

  states.addState("Расстыковка верхних соединителей", disconnectTopMergers);
  states.addState("Сворачивание верхних поршней", retractTopPistons);
  states.addState("Спуск верхней поддержки", capDown);
  states.addState("Включение верхних соединителей", connectTopMergers);
  states.addState("Разворачивание верхних поршней", expandTopPistons);
  states.addState("Завершение", finishWork);

  showStatus();
  speaker.play();

  return "Управление кротом";
}

public void main(string argument, UpdateType updateSource)
{
  if (argument == "start")
  {
    states.start(true);
  }
  else if(argument == "to_zero") { toZero(null); }
  else if(argument == "to_zero_stop") { stopDrillRotors(null); }
  else if(argument == "stats") { self.Runtime.UpdateFrequency = UpdateFrequency.Update100; }
  else { if(states.active()) { states.step(); } showStatus(); }
}

// helpers
public float getRotors45Angle()
{
  float r45 = drillRotors45.angle()+45f;
  return r45 >= 360f ? r45-360f : r45;
}

public float maxGradPerRun() { return drillRotorsRPM * 6f * (float)Runtime.TimeSinceLastRun.TotalSeconds * lastAngleFactor + ((lastAngleFactor-1) * 0.8f); }
public float getRotorsAngle() { return (drillRotors0.angle() + getRotors45Angle())/2; }

public void showStatus()
{
  int i = 0;
  statusLcd.echo_at($"Состояние системы: {boolToString(states.active())}", i++);
  if(states.active()) { statusLcd.echo_at($"Текущий шаг: {states.currentState().name()}", i++); }

  statusLcd.echo_at($"Углы роторов буров ({drillRotors0.count() + drillRotors45.count()}шт.): [0:{drillRotors0.angle():f2},45:{getRotors45Angle():f2}] -> [avg:{getRotorsAngle():f2},cont:{continuosRotorAngle:f2},max:{maxGradPerRun():f2}]", i++);
  statusLcd.echo_at($"Угол ротора инструмента({beltRotor.count()}шт.): {beltRotor.angle():f2}", i++);

  statusLcd.echo_at($"Длинна верхних поршней ({topPistons.count()}шт.): {topPistons.currentLength():f2}м.", i++);
  statusLcd.echo_at($"Длинна подвесных поршней ({platformPistons.count()}шт.): {platformPistons.currentLength():f2}м.", i++);
  statusLcd.echo_at($"Длинна нижних поршней ({bottomPistons.count()}шт.): {bottomPistons.currentLength():f2}м.", i++);
  statusLcd.echo_at($"Длинна сварочных поршней ({welderPistons.count()}шт.): {welderPistons.currentLength():f2}м.", i++);
  statusLcd.echo_at($"Длинна буровых поршней ({drillPistons.count()}шт.): {drillPistons.currentLength():f2}м.", i++);
  statusLcd.echo_at($"Длинна поршня коннектора ({connectorPiston.count()}шт.): {connectorPiston.currentLength():f2}м.", i++);

  statusLcd.echo_at($"Состояние верхних соединителей ({topMergers.count()}шт.): {boolToString(topMergers.connected())}", i++);
  statusLcd.echo_at($"Состояние нижних соединителей ({bottomMergers.count()}шт.): {boolToString(bottomMergers.connected())}", i++);

  statusLcd.echo_at($"Проектора тоннеля ({wallProjector.count()}шт.): {boolToString(wallProjector.enabled())}"   + (wallProjector.enabled() ? $"; ttl:{wallProjector.totalBlocks()}; rmng:{wallProjector.remainingBlocks()}; bldbl:{wallProjector.buildableBlocks()}; wldd:{wallProjector.weldedBlocks()}" : ""), i++);
  statusLcd.echo_at($"Проектора конвейера ({beltProjector.count()}шт.): {boolToString(beltProjector.enabled())}" + (beltProjector.enabled() ? $"; ttl:{beltProjector.totalBlocks()}; rmng:{beltProjector.remainingBlocks()}; bldbl:{beltProjector.buildableBlocks()}; wldd:{beltProjector.weldedBlocks()}" : ""), i++);

  statusLcd.echo_at($"Состояние коннектора ({platformConnector.count()}шт.): {boolToString(platformConnector.connected())}", i++);

  statusLcd.echo_at($"Состояние сварщиков стен ({welders.count()}шт.): {boolToString(welders.active())}", i++);
  statusLcd.echo_at($"Состояние сварщика конвейера ({beltWelder.count()}шт.): {boolToString(beltWelder.active())}", i++);
  statusLcd.echo_at($"Состояние резчика конвейера ({beltGrinder.count()}шт.): {boolToString(beltGrinder.active())}", i++);
  statusLcd.echo_at($"Состояние буров ({drills.count()}шт.): {boolToString(drills.active())}", i++);
}



// state machine
public bool toZero(object data)
{
  drillRotors0.setLimit(0f, 0f);
  drillRotors45.setLimit(45f, 45f);
  startDrillRotors(null);
  return true;
}

public bool prepareStart(object data)
{
  statusLcd.clear();
  logLcd.clear();
  drillRotors0.setLimit();
  drillRotors45.setLimit();
  return true;
}

public bool finishWork(object data)
{
  return stopDrillRotors(null);
}

public bool startDrillRotors(object data)
{
  drillRotors0.rotate(drillRotorsRPM);
  drillRotors45.rotate(drillRotorsRPM);
  drillRotors45.reverse();
  return true;
}

public bool stopDrillRotors(object data)
{
  drillRotors0.stop();
  drillRotors45.stop();
  return true;
}

public bool startDrill(object data) { return drills.on(); }
public bool stopDrill (object data) { return drills.off(); }

public bool startWelder(object data) { return welders.on(); }
public bool stopWelder (object data) { return welders.off(); }

public bool startBeltWelder(object data) { return beltWelder.on(); }
public bool stopBeltWelder (object data) { return beltWelder.off(); }

public bool startBeltGrinder(object data) { return beltGrinder.on(); }
public bool stopBeltGrinder (object data) { return beltGrinder.off(); }

public bool stepDrillPiston   (object data) { return drillPistons.expand((float)data, pistonsSpeed); }
public bool retractDrillPiston(object data) { return drillPistons.retract(0f, pistonsSpeed); }

public bool stepWelderPiston   (object data) { return welderPistons.expand((float)data, pistonsSpeed); }
public bool retractWelderPiston(object data) { return welderPistons.retract(0f, pistonsSpeed); }

public bool tunnelProjectorOn (object data) { return wallProjector.enable(); }
public bool tunnelProjectorOff(object data) { return wallProjector.disable(); }

public bool beltProjectorOn (object data) { return beltProjector.enable(); }
public bool beltProjectorOff(object data) { return beltProjector.disable(); }

public bool connectTopMergers   (object data) { return topMergers.enable(); }
public bool disconnectTopMergers(object data) { return topMergers.disable(); }

public bool expandTopPistons (object data) { return topPistons.expand(2.4f, pistonsSpeed); }
public bool retractTopPistons(object data) { return topPistons.retract(0f, pistonsSpeed); }

public bool connectBottomMergers   (object data) { return bottomMergers.enable(); }
public bool disconnectBottomMergers(object data) { return bottomMergers.disable(); }

public bool expandBottomPistons (object data) { return bottomPistons.expand(2.4f, pistonsSpeed); }
public bool retractBottomPistons(object data) { return bottomPistons.retract(0f, pistonsSpeed); }

public bool connectPlatformConnector   (object data) { return platformConnector.enable() && platformConnector.connect(); }
public bool disconnectPlatformConnector(object data) { return platformConnector.disconnect() && platformConnector.disable(); }

public bool expandConnectorPiston (object data) { return connectorPiston.expand(2.4f, pistonsSpeed); }
public bool retractConnectorPiston(object data) { return connectorPiston.retract(0f, pistonsSpeed); }

public bool waitWallWelders(object data) { return wallProjector.weldedBlocks() >= (int)data; }
public bool waitBeltGrinder(object data) { return beltProjector.buildableBlocks() > 0; }
public bool waitBeltWelder (object data) { return beltProjector.weldedBlocks() >= (int)data; }

public bool platformDown(object data)
{
  float l = (float)data;
  bool pp = platformPistons.expand(l, pistonsSpeed);
  bool wp = welderPistons.retract(10f - l, pistonsSpeed);
  return pp && wp;
}

public bool capDown(object data) { return platformPistons.retract(0f, pistonsSpeed); }

public bool rotateToWelder (object data) { beltRotor.setLimit(180f, 180f); beltRotor.rotate(5f); return true; }
public bool rotateToGrinder(object data) { beltRotor.setLimit(0f  , 0f  ); beltRotor.rotate(5f); return true; }
public bool stopToolRotate (object data) { beltRotor.stop(); return true; }
public bool waitToolRotateToWelder (object data) { int agl = (int)beltRotor.angle(); return agl == 180; }
public bool waitToolRotateToGrinder(object data) { int agl = (int)beltRotor.angle(); return agl == 0 || agl == 360; }


public bool prepareRotorsAngleCalc(object data)
{
  lastRotorsAngle = getRotorsAngle();
  continuosRotorAngle = 0f;
  return true;
}

public bool waitRotorsTurn(object data)
{
  float currAngle = getRotorsAngle();
  float angleDiff = currAngle >= lastRotorsAngle ? currAngle - lastRotorsAngle : (360f-lastRotorsAngle) + currAngle;
  if(angleDiff > maxGradPerRun()) { lastAngleFactor++; return false; }
  lastAngleFactor = 1;
  lastRotorsAngle = currAngle;
  continuosRotorAngle += angleDiff;
  return continuosRotorAngle >= stepRotorsAngle;
}
