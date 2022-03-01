// #include classes/main.cs
// #include parts/space_builder.cs
// #include classes/blocks/piston.cs
// #include classes/blocks/welder.cs

CPiston pistonsWorker;
CWelder weldersWorker;
const float rotorSpeedRPM = 0.8f;
const float pistonsWeldingSpeed = 0.5f;
const float pistonsBackSpeed = 1f;
float currentPistonsLength;

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  initGroups();
  pistonsWorker = new CPiston(pistons, 8);
  weldersWorker = new CWelder(welders);
  currentPistonsLength = 0f;
  return "Космострой";
}

public void main(string argument, UpdateType updateSource)
{
       if (argument.Contains("welders")) { enableWelders(argument.Contains("_on")); }
  else if (argument.Contains("rotor"  )) { enableRotor  (argument.Contains("_on")); }
  else if (argument.Contains("piston"))
  {
    if(argument.Contains("step")) { pistonsStep(); }
    else { pistonsBack(); }
  }
}

public void enableRotor(bool enable) { rotor.TargetVelocityRPM = enable ? rotorSpeedRPM : 0f; }
public void enableWelders(bool enable)
{
  projector.Enabled = enable;
  weldersWorker.enable(enable);
}
public void pistonsStep()
{
  currentPistonsLength += blockSize;
  pistonsWorker.expand(currentPistonsLength, pistonsWeldingSpeed);
}

public void pistonsBack()
{
  currentPistonsLength = 0f;
  pistonsWorker.retract(currentPistonsLength, pistonsBackSpeed);
}
