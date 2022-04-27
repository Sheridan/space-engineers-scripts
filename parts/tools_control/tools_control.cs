// #include classes/blocks/base/blocks_named.cs
// #include classes/blocks/base/blocks.cs
// #include classes/blocks/ship_tool.cs
// #include classes/blocks/sensor.cs
// #include classes/blocks/lamp.cs

CShipTool tools;
CSensor safetySensor;
CLamp safetyLamp;
bool humanActivated;

public bool toolsInit(object data)
{
  humanActivated = false;
  tools        = new CShipTool(new CBlocks<IMyShipToolBase      >(              ));
  safetySensor = new CSensor  (new CBlocksNamed<IMySensorBlock  >("Безопасность"));
  safetyLamp   = new CLamp    (new CBlocksNamed<IMyLightingBlock>("Безопасность"));
  return toolsAvailable();
}

public bool toolsDestroy(object data)
{
  tools        = null;
  safetySensor = null;
  safetyLamp   = null;
  return toolsAvailable();
}

public bool toolsAvailable()
{
  return tools        != null && !tools       .empty() &&
         safetyLamp   != null && !safetyLamp  .empty() &&
         safetySensor != null && !safetySensor.empty();
}

public bool toolsArgumentsParse(string argument)
{
  debug($"{toolsAvailable()}");
  if(toolsAvailable())
  {
    switch(argument)
    {
      case "on"     : { humanActivated = true;  on ();                  return true; }
      case "off"    : { humanActivated = false; off();                  return true; }
      case "try_on" : { if(humanActivated) { on (); }                return true; }
      case "try_off": { if(humanActivated) { off(); }                return true; }
      case "onoff"  : { if( toolsActivated()) { humanActivated = false; off(); } else { humanActivated = true; on(); } return true; }
    }
  }
  return false;
}

public bool on()
{
  return safetyLamp.enable(tools.on());
}

public bool off()
{
  return safetyLamp.enable(!tools.off());
}

public bool toolsActivated() { return tools.active(); }