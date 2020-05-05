// #include classes/main.cs
// #include classes/display.cs
// #include classes/state_machine.cs
// #include classes/blocks/piston.cs
// #include classes/blocks/merger.cs
// #include parts/spider_builder.cs

CDisplay lcd;
CStateMachine states;

CPiston weldersMergersPistonsWorker;
CMerger weldersMergersWorker;

CPiston supportMergersPistonsWorker;
CMerger supportMergersWorker;

float foundationPistonSpeed = 2.5f;
float foundationPistonLength = 2.3f;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lcd = new CDisplay();
  lcd.addDisplay("[Паук] Дисплей логов строительства 0", 0, 0);
  lcd.addDisplay("[Паук] Дисплей логов строительства 1", 1, 0);
  initGroups();
  weldersMergersPistonsWorker = new CPiston(weldersMergersPistons);
  weldersMergersWorker = new CMerger(weldersMergers);
  supportMergersPistonsWorker = new CPiston(supportMergersPistons);
  supportMergersWorker = new CMerger(supportMergers);
  states = new CStateMachine(lcd, 10);
  states.addState("Wakeup", wakeup);
  states.addState("Disconnect Welder Foundation", disconnectWelderFoundation);
  states.addState("Prepare Welding", prepareWelding);
  states.addState("Welding", welding);
  states.addState("Stop Welding", stopWelding);
  states.addState("Connect Welder Foundation", connectWelderFoundation);
  states.addState("Disconnect Support Foundation", disconnectSupportFoundation);
  states.addState("Move Base", moveBase);
  states.addState("Connect Support Foundation", connectSupportFoundation);
  states.addState("Sleep", sleep);
  return "Управление строительством";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument == "start") { states.start(); }
  else if (argument == "test")
  { disconnectWelderFoundation(); }
  // { disconnectWelderFoundation(); disconnectSupportFoundation(); }
  // { connectWelderFoundation(); connectSupportFoundation(); }
  else { states.step(); }
}

public bool wakeup()
{
  return true;
}

public bool disconnectWelderFoundation()
{
  return  weldersMergersWorker.disconnect() &&
          weldersMergersPistonsWorker.retract(0f, foundationPistonSpeed);
}

public bool prepareWelding()
{
  return true;
}

public bool welding()
{
  return true;
}

public bool stopWelding()
{
  return true;
}

public bool connectWelderFoundation()
{
  return  weldersMergersPistonsWorker.expand(foundationPistonLength, foundationPistonSpeed) &&
          weldersMergersWorker.connect();
}

public bool disconnectSupportFoundation()
{
  return  supportMergersWorker.disconnect() &&
          supportMergersPistonsWorker.retract(0f, foundationPistonSpeed);
}

public bool moveBase()
{
  return true;
}

public bool connectSupportFoundation()
{
  return  supportMergersPistonsWorker.expand(foundationPistonLength, foundationPistonSpeed) &&
          supportMergersWorker.connect();
}

public bool sleep()
{
  return true;
}
