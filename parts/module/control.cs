// #include classes/blocks/airvent.cs
// #include classes/blocks/door.cs
// #include classes/blocks/lamp.cs

string moduleName;
CAirVent airVents;
CLamp airLamp;

public Dictionary<string, CDoor> doors;

public void module_program()
{
  moduleName = prbOptions.getValue("generic", "module" , "");

  doors    = new Dictionary<string,CDoor>();
  airVents = new CAirVent(new CBlocksNamed<IMyAirVent>($"<{moduleName}>"));
  airLamp  = new CLamp   (new CBlocksNamed<IMyLightingBlock>($"<{moduleName}> Вр. прожектор Воздух"));
}

public bool module_main(string[] splitted)
{
  switch(splitted[0])
  {
    case "pressurization"   : { pressure(); } return true;
    case "depressurization" : { depressure(); } return true;
    case "keep_doors_closed": { keepDoorsClosed(); } return true;
    case "open_doors"       : { openDoors(moduleName + splitted[1], true ); } return true;
    case "close_doors"      : { openDoors(moduleName + splitted[1], false); } return true;
    case "timer"            : { moduleOnTimer(); } return true;
  }
  return false;
}
// ------ doors -------
public void loadDoors(string mn)
{
  CDoor drs = new CDoor(new CBlocksNamed<IMyDoor>($"<{mn}>"));
  if(!drs.empty()) { doors[mn] = drs; }
}

public void openDoors(string mn, bool d_open)
{
  if(doors[mn].enabled())
  {
    if(d_open) { doors[mn].open (); }
    else       { doors[mn].close(); }
  }
}

public void switchDoors(string mn) { openDoors(mn, !doors[mn].isOpen()); }
public void lockDoors(string mn, bool d_lock) { doors[mn].enable(d_lock); }
public void keepDoorsClosed() { if(airExists()) { closeDoors(); } }

// ------ doors -------
// ------ air -------
public void pressure  () { airVents.pressurize  (); }
public void depressure() { airVents.depressurize(); }
public bool airGood() { return airVents.oxygenLevel() > 50f; }
public bool airExists() { return airVents.oxygenLevel() > 0f; }
// ------ air -------

public void moduleOnTimer()
{
  keepDoorsClosed();
  airLamp.enable(!airGood());
  onTimer();
}
