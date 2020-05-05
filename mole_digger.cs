// #include classes/main.cs
// #include classes/display.cs
// #include helpers/bool.cs
// #include parts/mole_digger.cs
// #include classes/angle.cs

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

CAngle nextStepAngle;
const float stepAtEveryDegree = 100f;
public void incrementNextStepAngle(CAngle currentAngle)
{
  nextStepAngle = currentAngle + stepAtEveryDegree;
  lcd.echo($"Следующий угол шага: {currentAngle.ToString()} -> {nextStepAngle.ToString()}");
}

public bool canStep(CAngle currentAngle, float delta = 1f)
{
  lcd.echo_at($"Осталось до следующего шага: {(nextStepAngle - currentAngle).ToString()}", 0);
  if ((nextStepAngle - currentAngle) <= delta)
  {
    incrementNextStepAngle(currentAngle + delta);
    return true;
  }
  return false;
}

CDisplay lcd;
IMySoundBlock soundBlock;

const float pistonDrillVelocity = 0.1f;
const float pistonUpVelocity = 0.5f;

const float pistonDrillForce = 1000000f;
const float pistonUpForce = 500000f;
const int pistonsInStack = 3;

const float gyroscopeMaxPR = 0.01f;

float maxDrillLength;
float pistonStep;

IMyProgrammableBlock autoHorizont;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  autoHorizont = GridTerminalSystem.GetBlockWithName("[Крот] ПрБ Атоматический горизонт") as IMyProgrammableBlock;
  pistonStep = blockSize;
  maxDrillLength = pistonStep*10;
  lcd = new CDisplay();
  lcd.addDisplay("[Крот] Дисплей логов бурения 0", 0, 0);
  lcd.addDisplay("[Крот] Дисплей логов бурения 1", 1, 0);
  initGroups();
  soundBlock = GridTerminalSystem.GetBlockWithName("[Крот] Динамик") as IMySoundBlock;
  nextStepAngle = new CAngle(0);
  return "Управление бурением";
}

public void main(string argument, UpdateType updateSource)
{
  if(!parseArgumets(argument))
  {
    IMyMotorStator rotor = rotors.blocks()[0];
    if(rotor.TargetVelocityRPM > 0f)
    {
      float pitch = 0;
      float roll = 0;
      foreach (IMyGyro gyroscope in gyroscopes.blocks())
      {
        pitch += Math.Abs(gyroscope.Pitch);
        roll  += Math.Abs(gyroscope.Roll);
      }
      if(!pauseWork(pitch/gyroscopes.count() > gyroscopeMaxPR ||
                    roll /gyroscopes.count() > gyroscopeMaxPR)    &&
          canStep(CAngle.fromRad(rotor.Angle)))
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
  lcd.echo($"Шаг {pistonPosition/pistonStep:f0} на позицию {pistonPosition:f2}");
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
  { rotor.TargetVelocityRPM = enable ? drillRPM : 0f; }
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
  autoHorizont.TryRun("stop");
}

public void startWork()
{
  workPaused = false;
  incrementNextStepAngle(CAngle.fromRad(rotors.blocks()[0].Angle));
  turnDrills(true);
  turnRotors(true);
  autoHorizont.TryRun("start");
  // pistonsStep();
}

bool workPaused;
public bool pauseWork(bool wait)
{
  if(workPaused != wait)
  {
    turnDrills(!wait);
    turnRotors(!wait);
    workPaused = wait;
  }
  return wait;
}
