// #include classes/main.cs
// #include classes/blocks.cs
// #include classes/blocks/functional.cs
// #include classes/blocks/battery.cs
// #include classes/blocks/connector.cs
// #include classes/blocks/landing_gear.cs
// #include classes/blocks/tank.cs

public CFunctional<IMyGyro> gyroscopes;
public CFunctional<IMyThrust> thrusters;
public CFunctional<IMyLightingBlock> lamps;
public CFunctional<IMyRadioAntenna> antennas;
public CFunctional<IMyOreDetector> oreDetectors;
// public CFunctional<IMyShipToolBase> tools;
public CBattery battaryes;
public CConnector connectors;
public CLandingGear landGears;
public CTank tanks;

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
  connectors = new CConnector(new CBlocks<IMyShipConnector>("Главный"));
  landGears = new CLandingGear(new CBlocks<IMyLandingGear>());
  lamps = new CFunctional<IMyLightingBlock>(new CBlocks<IMyLightingBlock>());
  antennas = new CFunctional<IMyRadioAntenna>(new CBlocks<IMyRadioAntenna>());
  oreDetectors = new CFunctional<IMyOreDetector>(new CBlocks<IMyOreDetector>());
  //tools = new CFunctional<IMyShipToolBase>(new CBlocks<IMyShipToolBase>());
  tanks = new CTank(new CBlocks<IMyGasTank>());
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
  debug("On");
  battaryes.autocharge();
  tanks.disableStockpile();
  thrusters.enable();
  gyroscopes.enable();
  antennas.enable();
  oreDetectors.enable();
  if(pbAutoHorizont != null) { pbAutoHorizont.TryRun("start"); }
  lamps.enable();
  connectors.disconnect();
  landGears.unlockGear();
  connected = true;
}

public void turnOff()
{
  debug("Off");
  if(connectors.connect())
  {
    landGears.lockGear();
    //tools.disable();
    lamps.disable();
    if (pbAutoHorizont != null) { pbAutoHorizont.TryRun("stop"); }
    gyroscopes.disable();
    thrusters.disable();
    oreDetectors.disable();
    antennas.disable();
    tanks.enableStockpile();
    battaryes.recharge();
    connected = false;
  }
}
