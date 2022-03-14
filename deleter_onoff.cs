// #include classes/main.cs
// #include classes/blocks/collector.cs
// #include classes/blocks/lamp.cs
// #include classes/blocks/ship_tool.cs
// #include classes/blocks/base/blocks.cs

CLamp      lamps;
CShipTool  tools;
CCollector collectors;

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  lamps      = new CLamp     (new CBlocks<IMyLightingBlock>());
  tools      = new CShipTool (new CBlocks<IMyShipToolBase>());
  collectors = new CCollector(new CBlocks<IMyCollector>());
  return "Управление статусом стирателя";
}

public void main(string argument, UpdateType updateSource)
{
  switch(argument)
  {
    case "on" : { on (); } break;
    case "off": { off(); } break;
  }
}

public void on()
{
  lamps.enable();
  tools.enable();
  collectors.enable();
}

public void off()
{
  lamps.disable();
  tools.disable();
  collectors.disable();
}
