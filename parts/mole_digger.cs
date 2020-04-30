// #include classes/blocks_group.cs

public CBlockGroup<IMyShipDrill> drills;
public CBlockGroup<IMyPistonBase> pistons;
public CBlockGroup<IMyMotorStator> rotors;
public CBlockGroup<IMyGyro> gyroscopes;
const float drillRPM = 0.005f;

public void initGroups()
{
  rotors = new CBlockGroup<IMyMotorStator>("[Крот] Роторы буров", "Роторы");
  drills = new CBlockGroup<IMyShipDrill>("[Крот] Буры", "Буры");
  pistons = new CBlockGroup<IMyPistonBase>("[Крот] Поршни буров", "Поршни");
  gyroscopes = new CBlockGroup<IMyGyro>("[Крот] Гироскопы бура", "Гироскопы автогоризонта");
}
