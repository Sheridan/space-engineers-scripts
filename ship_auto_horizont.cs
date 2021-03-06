// #include classes/main.cs
// #include classes/blocks_group.cs
// #include classes/blockstatus_display.cs
// #include classes/ship_controller.cs
// #include classes/textsurface.cs
// #include helpers/bool.cs

CShipController controller;
CBlockGroup<IMyGyro> gyroscopes;
CTextSurface lcd;
IMyCockpit cockpit;
// bool autoHorizontEnabled;
float autoRotateYawRPM;
bool autorotate;

public EAHForwardDirection loadForwardDirection()
{
  switch (prbOptions.getValue("script", "forwardDirection", "forward"))
  {
    case "forward": return EAHForwardDirection.Forward;
    case "up": return EAHForwardDirection.Up;
  }
  return EAHForwardDirection.Forward;
}

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100; "[Жук] Кокпит" "[Жук] Гироскопы"
  autorotate = false;
  string cockpitName = prbOptions.getValue("script", "cockpitName", "");
  cockpit = self.GridTerminalSystem.GetBlockWithName(cockpitName) as IMyCockpit;
  lcd = new CTextSurface();
  int cockpitSurface = prbOptions.getValue("script", "cockpitSurface", 0);
  if(cockpitSurface >= 0) { lcd.setSurface(cockpit.GetSurface(cockpitSurface), 2f, 7, 30); }
  else { lcd.setSurface(Me.GetSurface(0), 2f, 7, 14);  }
  controller = new CShipController(
    self.GridTerminalSystem.GetBlockWithName(prbOptions.getValue("script", "controllerName", cockpitName)) as IMyShipController,
    loadForwardDirection());
  gyroscopes = new CBlockGroup<IMyGyro>(prbOptions.getValue("script", "gyrosGroupName", ""), "Гироскопы");
  return "Атоматический горизонт";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument.Length == 0)
  {
    controller.autoHorizont(autorotate ? autoRotateYawRPM : cockpit.RotationIndicator.Y);
  }
  else
  {
         if (argument == "check"     ) { controller.checkAutoHorizont(); }
    else if (argument == "start"     ) { if(controller.autoHorizontIsEnabled()) { disableAH(); } else { enableAH(); } }
    else if (argument == "stop"      ) { disableAH(); }
    else if (argument == "restart"   ) { disableAH(); enableAH(); }
    else if (argument == "autorotate") { autorotate = !autorotate; }
    lcd.echo_at($"АГ: {boolToString(controller.autoHorizontIsEnabled())}", 0);
    lcd.echo_at($"Авто: {boolToString(autorotate)}", 1);
    lcd.echo_at($"Гир: {gyroscopes.count()}", 2);
  }
}

public void enableAH()
{
  autoRotateYawRPM = prbOptions.getValue("script", "autorotateRPM", 0f);
  controller.enableAutoHorizont(gyroscopes);
  Runtime.UpdateFrequency = UpdateFrequency.Update1;
}

public void disableAH()
{
  controller.disableAutoHorizont();
  Runtime.UpdateFrequency = UpdateFrequency.None;
}
