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

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100; "[Жук] Кокпит" "[Жук] Гироскопы"
  autorotate = false;
  cockpit = self.GridTerminalSystem.GetBlockWithName(prbOptions.getValue("script", "cockpitName", "")) as IMyCockpit;
  lcd = new CTextSurface();
  lcd.setSurface(cockpit.GetSurface(prbOptions.getValue("script", "cockpitSurface", 0)), 2f, 7, 30);
  controller = new CShipController(cockpit as IMyShipController);
  gyroscopes = new CBlockGroup<IMyGyro>(prbOptions.getValue("script", "gyrosGroupName", ""), "Гироскопы");
  return "Атоматический горизонт";
}

public void main(string argument, UpdateType updateSource)
{
  if(argument == "")
  {
    controller.autoHorizont(autorotate ? autoRotateYawRPM : cockpit.RotationIndicator.Y);
  }
  else
  {
    if (argument == "start")
    {
      autoRotateYawRPM = prbOptions.getValue("script", "autorotateRPM", 0.35f);
      controller.enableAutoHorizont(gyroscopes);
      Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }
    else if (argument == "stop")
    {
      controller.disableAutoHorizont();
      Runtime.UpdateFrequency = UpdateFrequency.None;
    }
    else
    {
      autorotate = argument == "autorotate";
    }
    lcd.echo_at($"АГ: {boolToString(controller.autoHorizontIsEnabled())}", 0);
    lcd.echo_at($"Авто: {boolToString(autorotate)}", 1);
    lcd.echo_at($"Гир: {gyroscopes.count()}", 2);
  }
}
