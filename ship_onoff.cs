// #include classes/main.cs
// #include classes/blocks.cs
// #include classes/blocks/functional.cs
// #include classes/blocks/battery.cs
// #include classes/blocks/connector.cs

public CFunctional<IMyGyro> gyroscopes;
public CFunctional<IMyThrust> thrusters;
public CFunctional<IMyLightingBlock> lamps;
public CBattery battaryes;
public CConnector connectors;
IMyProgrammableBlock pbAutoHorizont;

bool connected;
public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update10;
  // connector = self.GridTerminalSystem.GetBlockWithName($"[{structureName}] Коннектор") as IMyShipConnector;
  pbAutoHorizont = self.GridTerminalSystem.GetBlockWithName($"[{structureName}] ПрБ Атоматический горизонт") as IMyProgrammableBlock;
  gyroscopes = new CFunctional<IMyGyro>(new CBlocks<IMyGyro>());
  thrusters = new CFunctional<IMyThrust>(new CBlocks<IMyThrust>());
  battaryes = new CBattery(new CBlocks<IMyBatteryBlock>());
  connectors = new CConnector(new CBlocks<IMyShipConnector>());
  lamps = new CFunctional<IMyLightingBlock>(new CBlocks<IMyLightingBlock>());
  connected = true;
  return "Управление стыковкой корабля";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument == "start")
  {
    if (connected) { turnOff(); } else { turnOn(); }
  }
}

public void turnOn()
{
  battaryes.autocharge();
  thrusters.enable();
  gyroscopes.enable();
  if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("start"); }
  lamps.enable();
  connectors.disconnect();
  connected = true;
}

public void turnOff()
{
  connectors.connect();
  lamps.disable();
  if (pbAutoHorizont != null) { pbAutoHorizont.TryRun("stop"); }
  gyroscopes.disable();
  thrusters.disable();
  battaryes.recharge();
  connected = false;
}
