// #include classes/main.cs
// #include classes/blocks_group.cs
// #include classes/blockstatus_display.cs
// #include classes/ship_controller.cs
// #include parts/mole_digger.cs

CShipController controller;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  controller = new CShipController("[Крот] Д.У. Буров");
  initGroups();
  return "Атоматический горизонт";
}

public void main(string argument, UpdateType updateSource)
{
  if (argument.Length == 0) { controller.autoHorizont(-drillRPM); }
  else
  {
    if (argument == "start")
    {
      controller.enableAutoHorizont(gyroscopes);
      Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }
    else
    {
      controller.disableAutoHorizont();
      Runtime.UpdateFrequency = UpdateFrequency.None;
    }
  }
}
