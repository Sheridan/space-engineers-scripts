// #include classes/main.cs
// #include classes/blocks.cs
// #include classes/blocks/rotor.cs
// #include classes/blocks/piston.cs
// #include classes/blocks/merger.cs
// #include classes/blocks/ship_tool.cs
// #include classes/blocks/projector.cs
// #include classes/state_machine.cs
// #include classes/display.cs
// #include helpers/bool.cs

float drillRotorsRPM = 0.5f;

float wallHeight = 4f * 2.5f;
float drillSteps = 20f;
float drillPistonsStep;
float drillPistonsSpeed = 1f;
float lastRotorsAngle = 0f;
float stepRotorsAngle = 190f;
float continuosRotorAngle = 0f;

int lastAngleFactor = 1;

CDisplay logLcd;
CDisplay statusLcd;
CRotor drillRotors0;
CRotor drillRotors45;
CPiston drillPistons;
CPiston welderPistons;
CPiston platformPistons;
CShipTool drills;
CShipTool welders;
CMerger topMergers;
CMerger bottomMergers;
CProjector wallProjector;

CStateMachine states;

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  drillPistonsStep = wallHeight / drillSteps;

  logLcd = new CDisplay();
  logLcd.addDisplay($"[{structureName}] Дисплей Лог 1", 0, 0);
  logLcd.addDisplay($"[{structureName}] Дисплей Лог 0", 1, 0);

  statusLcd = new CDisplay();
  statusLcd.addDisplay($"[{structureName}] Дисплей Статус 0", 0, 0);

  drillRotors0     = new CRotor    (new CBlocks<IMyMotorStator   >("Инструмент (0)"));
  drillRotors45    = new CRotor    (new CBlocks<IMyMotorStator   >("Инструмент (45)"));
  drillPistons     = new CPiston   (new CBlocks<IMyPistonBase    >("Бур"));
  welderPistons    = new CPiston   (new CBlocks<IMyPistonBase    >("Сварщик"));
  platformPistons  = new CPiston   (new CBlocks<IMyPistonBase    >("Подвес"));
  drills           = new CShipTool (new CBlocks<IMyShipToolBase  >("Бур"));
  welders          = new CShipTool (new CBlocks<IMyShipToolBase  >("Сварщик"));
  topMergers       = new CMerger   (new CBlocks<IMyShipMergeBlock>("Верхний"));
  bottomMergers    = new CMerger   (new CBlocks<IMyShipMergeBlock>("Нижний"));
  wallProjector    = new CProjector(new CBlocks<IMyProjector     >("Тоннель"));

  states = new CStateMachine(logLcd);
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
// welded: 1: 120 2:240 3:360 4:480
  showStatus();

  return "Управление кротом";
}

public void main(string argument, UpdateType updateSource)
{
  if (argument == "start")
  {
    states.start(true);
  }
  else if(argument == "to_zero") { toZero(); }
  else if(argument == "to_zero_stop") { stopDrillRotors(null); }
  else if(argument == "rotate") { prepareStart(null); startDrillRotors(null); }
  else if(argument == "stats") { self.Runtime.UpdateFrequency = UpdateFrequency.Update100; }
  else { if(states.active()) { states.step(); } showStatus(); }
}

// helpers
public float maxGradPerRun()
{
  return drillRotorsRPM * 6f * (float)Runtime.TimeSinceLastRun.TotalSeconds * lastAngleFactor + ((lastAngleFactor-1) * 0.5f);
}

public float getRotors45Angle()
{
  float r45 = drillRotors45.angle()+45f;
  return r45 >= 360f ? r45-360f : r45;
}

public float getRotorsAngle()
{
  return (drillRotors0.angle() + getRotors45Angle())/2;
}

public void showStatus()
{
  int i = 0;
  statusLcd.echo_at($"Состояние системы: {boolToString(states.active())}", i++);
  if(states.active()) { statusLcd.echo_at($"Текущий шаг: {states.currentState().name()}", i++); }
  statusLcd.echo_at($"Состояние буров: {boolToString(drills.active())}", i++);
  statusLcd.echo_at($"Углы роторов: [0:{drillRotors0.angle():f2},45:{getRotors45Angle():f2}] -> [avg:{getRotorsAngle():f2},cont:{continuosRotorAngle:f2},max:{maxGradPerRun():f2}]", i++);
  statusLcd.echo_at($"Длинна буровых поршней: {drillPistons.currentLength():f2}", i++);
  statusLcd.echo_at($"Состояние сварщиков: {boolToString(welders.active())}", i++);
  statusLcd.echo_at($"Длинна сварочных поршней: {welderPistons.currentLength():f2}", i++);
  statusLcd.echo_at($"Длинна подвесных поршней: {platformPistons.currentLength():f2}", i++);
  statusLcd.echo_at($"Состояние верхних соединителей: {boolToString(topMergers.connected())}", i++);
  statusLcd.echo_at($"Состояние нижних соединителей: {boolToString(bottomMergers.connected())}", i++);
  statusLcd.echo_at($"Состояние проектора тоннеля: {boolToString(wallProjector.enabled())}; total:{wallProjector.totalBlocks()}; remaining:{wallProjector.remainingBlocks()}; welded:{wallProjector.weldedBlocks()}", i++);
}

public void toZero()
{
  drillRotors0.setLimit(0f, 0f);
  drillRotors45.setLimit(45f, 45f);
  startDrillRotors(null);
}

// state machine
public bool prepareStart(object data)
{
  statusLcd.clear();
  logLcd.clear();
  drillRotors0.setLimit();
  drillRotors45.setLimit();
  return true;
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
public bool stopDrill(object data) { return drills.off(); }

public bool stepDrillPiston(object data)
{
  return drillPistons.expand((float)data, drillPistonsSpeed);
}

public bool retractDrillPiston(object data)
{
  return drillPistons.retract(0f, drillPistonsSpeed);
}

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
