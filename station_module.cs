// #include classes/main.cs
// #include classes/blocks/base/blocks_named.cs
// #include classes/display.cs
// #include helpers/bool.cs
// #include classes/blocks/airvent.cs
// #include classes/blocks/door.cs
// #include classes/blocks/merger.cs
// #include classes/blocks/lamp.cs

// #include parts/module/control.cs

string moduleName;
CAirVent airVents;
CLamp airLamp;

CDisplay leafDoorsStatusLcd;
CDisplay airStatusLcd;

public Dictionary<string, CMerger> mergers;
public string[] leafesNames;
public string[] gatesPlacement;

public string program()
{
  module_program();

  leafDoorsStatusLcd = new CDisplay();
  leafDoorsStatusLcd.addDisplays($"<{moduleName}> Дисплей Двери");

  airStatusLcd = new CDisplay();
  airStatusLcd.addDisplays($"<{moduleName}>  Дисплей Воздух");


  leafesNames    = new string[] {"a","b","c","d"};
  gatesPlacement = new string[] {"up","bottom"};
  mergers        = new Dictionary<string,CMerger>();

  foreach(string l in leafesNames)
  {
    foreach(string dp in doorsPlacement)
    {
      foreach(string mn in new string[] { $"{moduleName}{l}:{dp}", $"{moduleName}{l}:gate:{dp}" })
      {
        loadDoors(mn);
      }
    }
    foreach(string g in gatesPlacement)
    {
      loadDoors($"{moduleName}{l}:gate:{g}");
    }
    loadDoors($"{moduleName}{l}");
  }

  return $"<{moduleName}> Обслуживание модуля";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument.Length > 0)
  {
    string[] splitted = argument.Split(' ');
    if(!module_main(splitted))
    {
      switch(splitted[0])
      {
        case "lock_doors"       : { lockDoors(moduleName + splitted[1], true ); } break;
        case "unlock_doors"     : { lockDoors(moduleName + splitted[1], false); } break;
        case "lock_mergers"     : { lockMergers(moduleName + splitted[1], false ); } break;
        case "unlock_mergers"   : { lockMergers(moduleName + splitted[1], true); } break;
        case "close_gates"      : { openGates(splitted[1], false); } break;
        case "open_gates"       : { openGates(splitted[1], true ); } break;
        case "switch_gates"     : { switchGates(splitted[1], splitted[2]); } break;
        case "lock_gates"       : { lockGates(splitted[1], false ); } break;
        case "unlock_gates"     : { lockGates(splitted[1], true); } break;
        case "open_leaf_doors"  : { openLeafDoors(splitted[1], true ); } break;
        case "close_leaf_doors" : { openLeafDoors(splitted[1], false); } break;
        case "lock_leaf_doors"  : { lockLeafDoors(splitted[1], false ); } break;
        case "unlock_leaf_doors": { lockLeafDoors(splitted[1], true); } break;
      }
    }
  }
}

public void closeDoors()
{
  foreach(string l in leafesNames)
  {
    foreach(string dp in doorsPlacement)
    {
      foreach(string mn in new string[] { $"{moduleName}{l}:{dp}", $"{moduleName}{l}:gate:{dp}" })
      {
        openDoors(mn, false);
      }
    }
  }
}

public void closeHangarDoors()
{
  foreach(string l in leafesNames)
  {
    foreach(string g in gatesPlacement)
    {
      openDoors($"{moduleName}{l}:gate:{g}", false);
    }
  }
}

public void lockLeafDoors(string side, bool d_lock)
{
  foreach(string dp in doorsPlacement)
  {
    lockDoors($"{moduleName}{side}:{dp}", d_lock);
  }
}

public void lockGates(string side, bool d_lock)
{
  foreach(string l in leafesNames)
  {
    lockDoors($"{moduleName}{l}:gate:{side}", d_lock);
  }
}

public void openLeafDoors(string side, bool d_open)
{
  foreach(string dp in doorsPlacement)
  {
    openDoors($"{moduleName}{side}:{dp}", d_open);
  }
}

public void openGates(string side, bool d_open)
{
  foreach(string l in leafesNames)
  {
    openDoors($"{moduleName}{l}:gate:{side}", d_open);
  }
}

public void switchGates(string leaf, string side)
{
  switchDoors($"{moduleName}{leaf}:gate:{side}");
}

public void lockMergers(string mn, bool m_lock)
{
  mergers[mn].enable(m_lock);
}

public void onTimer()
{
  status();
}

public void status()
{
  int ldsI = 0;
  leafDoorsStatusLcd.echo_at("Статус дверей листьев", ldsI++);
  foreach(string l in leafesNames)
  {
    string mn_i = $"{moduleName}{l}:in";
    string mn_o = $"{moduleName}{l}:out";
    leafDoorsStatusLcd.echo_at($"{l}:[{boolToString(doors[mn_i].enabled(), EBoolToString.btsOnOff)},{boolToString(doors[mn_i].isOpen(), EBoolToString.btsOpenClose)}], [{boolToString(doors[mn_o].enabled(), EBoolToString.btsOnOff)},{boolToString(doors[mn_o].isOpen(), EBoolToString.btsOpenClose)}]", ldsI++);
  }

  int asI = 0;
  airStatusLcd.echo_at("Статус воздуха", asI++);
  airStatusLcd.echo_at($"Давление: {airVents.oxygenLevel():f2}%", asI++);
  airStatusLcd.echo_at($"Наличие утечек: {boolToString(!airVents.canPressurize(), EBoolToString.btsYesNo)}", asI++);
}