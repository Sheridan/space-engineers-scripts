// #include classes/main.cs
// #include classes/blocks/base/blocks_named.cs
// #include classes/display.cs
// #include helpers/bool.cs
// #include classes/blocks/merger.cs

// #include parts/module/control.cs

public string[] doorsPlacement;

public string program()
{
  module_program();
  doorsPlacement = new string[] {"in","out"};

  foreach(string dp in doorsPlacement)
  {
   loadDoors($"{moduleName}:{dp}");
  }
  return $"<{moduleName}> Обслуживание листа";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument.Length > 0)
  {
    string[] splitted = argument.Split(' ');
    if(!module_main(splitted))
    {
      // switch(splitted[0])
      // {
      //   case "pressurization"   : { pressure(); } break;
      //   case "depressurization" : { depressure(); } break;
      //   case "keep_doors_closed": { keepDoorsClosed(); } break;
      //   case "open_doors"       : { openDoors(moduleName + splitted[1], true ); } break;
      //   case "close_doors"      : { openDoors(moduleName + splitted[1], false); } break;
      // }
    }
  }
}

public void closeDoors()
{
  foreach(string dp in doorsPlacement)
  {
    openDoors($"{moduleName}:{dp}", false);
  }
}

public void onTimer()
{}
