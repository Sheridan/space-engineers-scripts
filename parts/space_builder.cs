// #include classes/blocks_group.cs

public CBlockGroup<IMyPistonBase> pistons;
public CBlockGroup<IMyShipWelder> welders;
IMyProjector projector;
IMyMotorStator rotor;

public void initGroups()
{
  pistons = new CBlockGroup<IMyPistonBase>("[Космос] Поршни космостроя", "Поршни");
  welders = new CBlockGroup<IMyShipWelder>("[Космос] Сварщики космостроя", "Сварщики");
  projector = GridTerminalSystem.GetBlockWithName("[Космос] Проектор космостроя") as IMyProjector;
  rotor = GridTerminalSystem.GetBlockWithName("[Космос] У.Ротор Космострой 0") as IMyMotorStator;
}
