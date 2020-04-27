// #include classes/main.cs
// #include classes/display.cs
// #include helpers/bool.cs
// #include parts/mole_digger.cs

public void expandPistons(CBlockGroup<IMyPistonBase> pistons,
                          float length,
                          float velocity,
                          float force,
                          int stackSize = 1)
{
  float realLength = length / stackSize;
  float realVelocity = velocity / stackSize;
  float currentPosition = 0;
  foreach (IMyPistonBase piston in pistons.blocks())
  {
    switch (piston.Status)
    {
      case PistonStatus.Stopped:
      case PistonStatus.Retracted:
      case PistonStatus.Retracting:
      case PistonStatus.Extended:
        {
          if (piston.CurrentPosition < realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = 0f;
            piston.MaxLimit = realLength;
            piston.SetValue<float>("MaxImpulseAxis", force);
            piston.SetValue<float>("MaxImpulseNonAxis", force);
            piston.Extend();
          }
        }
        break;
    }
    currentPosition += piston.CurrentPosition;
  }
  currentPosition = currentPosition / pistons.count();
  lcd.echo($"[{pistons.purpose()}] Выдвигаются до {currentPosition:f2}->{realLength:f2}");
}

public void retractPistons(CBlockGroup<IMyPistonBase> pistons,
                          float minLength,
                          float velocity,
                          float force,
                          int stackSize = 1)
{
  float realLength = minLength / stackSize;
  float realVelocity = velocity / stackSize;
  float currentPosition = 0;
  foreach (IMyPistonBase piston in pistons.blocks())
  {
    switch (piston.Status)
    {
      case PistonStatus.Stopped:
      case PistonStatus.Extended:
      case PistonStatus.Extending:
      case PistonStatus.Retracted:
        {
          if (piston.CurrentPosition > realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = realLength;
            piston.MaxLimit = 10f;
            piston.SetValue<float>("MaxImpulseAxis", force);
            piston.SetValue<float>("MaxImpulseNonAxis", force);
            piston.Retract();
          }
        }
        break;
    }
    currentPosition += piston.CurrentPosition;
  }
  currentPosition = currentPosition / pistons.count();
  lcd.echo($"[{pistons.purpose()}] Задвигаются до {currentPosition:f2}->{realLength:f2}");
}

public void playSound(string name)
{
  soundBlock.SelectedSound = name;
  soundBlock.Play();
}

float lastAngle;
public bool canStep(float angle, float currentAngle, float delta = 1f)
{
  float minAngle = angle - delta;
  float maxAngle = angle + delta;
  bool result = currentAngle > minAngle && currentAngle < maxAngle;
  if (result && lastAngle != angle)
  {
    lastAngle = angle;
    return true;
  }
  return false;
}

CDisplay lcd;
IMySoundBlock soundBlock;

const float rotorDrillVelocity = 0.10f;

const float pistonDrillVelocity = 0.1f;
const float pistonUpVelocity = 0.5f;

const float pistonDrillForce = 1000000f;
const float pistonUpForce = 500000f;
const int pistonsInStack = 3;

float maxDrillLength;
float pistonStep;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  pistonStep = blockSize;
  maxDrillLength = pistonStep*10;
  lcd = new CDisplay();
  lcd.addDisplay("[Крот] Дисплей логов бурения 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей логов бурения 1", 1, 0);
  initGroups();
  soundBlock = GridTerminalSystem.GetBlockWithName("[Крот] Динамик") as IMySoundBlock;
  return "Управление бурением";
}

public void main(string argument, UpdateType updateSource)
{
  if(!parseArgumets(argument))
  {
    IMyMotorStator rotor = rotors.blocks()[0];
    if(rotor.TargetVelocityRPM > 0f)
    {
      float currentAngle = rotor.Angle * 180 / (float)Math.PI;
      if (canStep(10f, currentAngle) || canStep(130f, currentAngle) || canStep(250f, currentAngle))
      {
        bool pistonsAtMaxLength = true;
        foreach (IMyPistonBase piston in pistons.blocks())
        {
          pistonsAtMaxLength = pistonsAtMaxLength && piston.CurrentPosition >= maxDrillLength/pistonsInStack;
        }
        if(!pistonsAtMaxLength) { pistonsStep(); }
        else                    { stopWork   (); }
      }
    }
  }
}

float pistonPosition = 0f;
public bool parseArgumets(string argument)
{
  if(argument.Length == 0) { return false; }
  if      (argument == "go")           { startWork(); }
  else if (argument == "stop")         { stopWork (); }
  else if (argument.Contains("power")) { turnDrills(argument.Contains("_on")); }
  else if (argument.Contains("rotor")) { turnRotors(argument.Contains("_start")); }
  else if (argument.Contains("piston"))
  {
    if      (argument.Contains("_up"  )) { pistonsUp()  ; }
    else if (argument.Contains("_step")) { pistonsStep(); }
  }
  return true;
}

public void pistonsStep(int steps = 1)
{
  playSound("Operation Alarm");
  pistonPosition += pistonStep*steps;
  expandPistons(pistons, pistonPosition, pistonDrillVelocity, pistonDrillForce, pistonsInStack);
}

public void pistonsUp()
{
  retractPistons(pistons, 0f, pistonUpVelocity, pistonUpForce, pistonsInStack);
  pistonPosition = 0f;
}

public void turnRotors(bool enable)
{
  lcd.echo($"Вращение ({rotors.count()} шт.): {boolToString(enable, EBoolToString.btsOnOff)}");
  foreach (IMyMotorStator rotor in rotors.blocks())
  { rotor.TargetVelocityRPM = enable ? rotorDrillVelocity : 0f; }
}

public void turnDrills(bool enable)
{
  lcd.echo($"Питание буров ({drills.count()} шт.): {boolToString(enable, EBoolToString.btsOnOff)}");
  foreach (IMyShipDrill drill in drills.blocks()) { drill.Enabled = enable; }
}

public void stopWork()
{
  turnDrills(false);
  turnRotors(false);
  pistonsUp();
}

public void startWork()
{
  turnDrills(true);
  turnRotors(true);
  // pistonsStep();
}
