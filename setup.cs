// #include classes/main.cs
// #include classes/blocks.cs
// #include classes/blocks_group.cs
// #include classes/blocks_typed.cs
// #include classes/block_options.cs

// public CBlockGroup<IMyPistonBase> weldersMergersPistons;
// public CBlockGroup<> logisticConnectors;
// public CBlockGroup<IMyProjector> projectors;

public string program() { return "Настройка структуры"; }

public int gIndex;

public void main(string argument, UpdateType updateSource)
{
  if(Runtime.UpdateFrequency == UpdateFrequency.Update100)
  {
    step(gIndex++);
  } else if (argument == "start")
  {
    gIndex = 0;
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
  } else if (argument == "stop") { gIndex = 1024; }
}

public void step(int index)
{
  echoMeBig(index.ToString());
  switch (index)
  {
    case 0 : (new CBlocks<IMyPistonBase>              (                          )).setup("Поршень"); break;
    case 1 : (new CBlocks<IMyBatteryBlock>            (                          )).setup("Батарея"); break;
    case 2 : (new CBlocks<IMySolarPanel>              (                          )).setup("С.Батарея"); break;
    case 3 : (new CBlocks<IMyRemoteControl>           (                          )).setup("ДУ", true, false, true); break;
    case 4 : (new CBlocks<IMyOreDetector>             (                          )).setup("Детектор руды"); break;
    case 5 : (new CBlocks<IMyLandingGear>             (                          )).setup("Шасси"); break;
    case 6 : (new CBlocks<IMyShipDrill>               (                          )).setup("Бур"); break;
    case 7 : (new CBlocks<IMyShipGrinder>             (                          )).setup("Резак"); break;
    case 8 : (new CBlocks<IMyShipWelder>              (                          )).setup("Сварщик"); break;
    case 9 : (new CBlocks<IMyGyro>                    (                          )).setup("Гироскоп"); break;
    case 10: (new CBlocks<IMyCollector>               (                          )).setup("Коллектор"); break;
    case 11: (new CBlocks<IMyGasGenerator>            (                          )).setup("H2:O2 Генератор"); break;
    case 12: (new CBlocks<IMyOxygenFarm>              (                          )).setup("Ферма O2"); break;
    case 13: (new CBlocks<IMyShipMergeBlock>          (                          )).setup("Соединитель"); break;
    case 14: (new CBlocks<IMyRefinery>                (                          )).setup("Очистительный завод"); break;
    case 15: (new CBlocks<IMyMedicalRoom>             (                          )).setup("Медпост"); break;
    case 16: (new CBlocks<IMySmallGatlingGun>         (                          )).setup("М.Пушка"); break;
    case 17: (new CBlocks<IMyLargeGatlingTurret>      (                          )).setup("Б.Пушка"); break;
    case 18: (new CBlocks<IMyLargeMissileTurret>      (                          )).setup("Б.Ракетница"); break;
    case 19: (new CBlocks<IMyDoor>                    (                          )).setup("Дверь"); break;
    case 20: (new CBlocks<IMyAirVent>                 (                          )).setup("Вентиляция", false, false, true); break;
    case 21: (new CBlocks<IMyCameraBlock>             (                          )).setup("Камера", false, false, true); break;
    case 22: (new CBlocks<IMyButtonPanel>             (                          )).setup("Кнопки"); break;
    case 23: (new CBlocks<IMyRadioAntenna>            (                          )).setup("Антенна"); break;
    case 24: (new CBlocks<IMyLaserAntenna>            (                          )).setup("Л.Антенна"); break;
    case 25: (new CBlocks<IMyTextPanel>               (                          )).setup("Дисплей"); break;
    case 26: (new CBlocks<IMyShipConnector>           (                          )).setup("Коннектор", false, false, true); break;
    case 27: (new CBlocks<IMyTimerBlock>              (                          )).setup("Таймер", true, false, true); break;
    case 28: (new CBlocks<IMySensorBlock>             (                          )).setup("Сенсор"); break;
    case 29: (new CBlocksTyped<IMyPowerProducer>      ("HydrogenEngine"          )).setup("H2 Электрогенератор"); break;
    case 30: (new CBlocksTyped<IMyPowerProducer>      ("WindTurbine"             )).setup("Ветрогенератор"); break;
    case 31: (new CBlocksTyped<IMyThrust>             ("LargeAtmosphericThrust"  )).setup("БАУ"); break;
    case 32: (new CBlocksTyped<IMyThrust>             ("SmallAtmosphericThrust"  )).setup("АУ"); break;
    case 33: (new CBlocksTyped<IMyThrust>             ("LargeHydrogenThrust"     )).setup("БВУ"); break;
    case 34: (new CBlocksTyped<IMyThrust>             ("SmallHydrogenThrust"     )).setup("ВУ"); break;
    case 35: (new CBlocksTyped<IMyThrust>             ("LargeThrust"             )).setup("БИУ"); break;
    case 36: (new CBlocksTyped<IMyThrust>             ("SmallThrust"             )).setup("ИУ"); break;
    case 37: (new CBlocksTyped<IMyGasTank>            ("OxygenTankSmall"         )).setup("Бак O2"); break;
    case 38: (new CBlocksTyped<IMyGasTank>            ("OxygenTank/"             )).setup("Б.Бак O2"); break;
    case 39: (new CBlocksTyped<IMyGasTank>            ("/LargeHydrogenTank"      )).setup("ОБ.Бак H2"); break;
    case 40: (new CBlocksTyped<IMyGasTank>            ("/LargeHydrogenTankSmall" )).setup("Б.Бак H2"); break;
    case 41: (new CBlocksTyped<IMyGasTank>            ("/SmallHydrogenTank"      )).setup("Бак H2"); break;
    case 42: (new CBlocksTyped<IMyGasTank>            ("/SmallHydrogenTankSmall" )).setup("Бак H2"); break;
    case 43: (new CBlocksTyped<IMyCargoContainer>     ("SmallContainer"          )).setup("МК", false, true); break;
    case 44: (new CBlocksTyped<IMyCargoContainer>     ("MediumContainer"         )).setup("СК", false, true); break;
    case 45: (new CBlocksTyped<IMyCargoContainer>     ("LargeContainer"          )).setup("БК", false, true); break;
    case 46: (new CBlocksTyped<IMyCargoContainer>     ("LargeIndustrialContainer")).setup("БК", false, true); break;
    case 47: (new CBlocks     <IMyConveyorSorter>     (                          )).setup("Сортировщик", false, false, true); break;
    case 48: (new CBlocksTyped<IMyUpgradeModule>      ("ProductivityModule"      )).setup("М.Продуктивности"); break;
    case 49: (new CBlocksTyped<IMyUpgradeModule>      ("EffectivenessModule"     )).setup("М.Эффективности"); break;
    case 50: (new CBlocksTyped<IMyUpgradeModule>      ("EnergyModule"            )).setup("М.Энергоэффективности"); break;
    case 51: (new CBlocksTyped<IMyInteriorLight>      ("SmallLight"              )).setup("Лампа"); break;
    case 52: (new CBlocksTyped<IMyInteriorLight>      ("Light_1corner"           )).setup("Угл. Лампа"); break;
    case 53: (new CBlocksTyped<IMyInteriorLight>      ("Light_2corner"           )).setup("2хУгл. Лампа"); break;
    case 54: (new CBlocksTyped<IMyReflectorLight>     ("FrontLight"              )).setup("Прожектор"); break;
    case 55: (new CBlocksTyped<IMyReflectorLight>     ("RotatingLight"           )).setup("Вр. прожектор"); break;
    case 56: (new CBlocksTyped<IMyMotorSuspension>    ("Suspension1x1"           )).setup("Колесо 1x1 правое"); break;
    case 57: (new CBlocksTyped<IMyMotorSuspension>    ("Suspension3x3"           )).setup("Колесо 3x3 правое"); break;
    case 58: (new CBlocksTyped<IMyMotorSuspension>    ("Suspension5x5"           )).setup("Колесо 5x5 правое"); break;
    case 59: (new CBlocksTyped<IMyMotorSuspension>    ("Suspension1x1mirrored"   )).setup("Колесо 1x1 левое"); break;
    case 60: (new CBlocksTyped<IMyMotorSuspension>    ("Suspension3x3mirrored"   )).setup("Колесо 3x3 левое"); break;
    case 61: (new CBlocksTyped<IMyMotorSuspension>    ("Suspension5x5mirrored"   )).setup("Колесо 5x5 левое"); break;
    case 62: (new CBlocksTyped<IMyCockpit>            ("LargeBlockCouch"         )).setup("Диван"); break;
    case 63: (new CBlocksTyped<IMyCockpit>            ("LargeBlockCouchCorner"   )).setup("Угл. Диван"); break;
    case 64: (new CBlocksTyped<IMyAssembler>          ("LargeAssembler"          )).setup("Сборщик"); break;
    case 65: (new CBlocks     <IMyProjector>          (                          )).setup("Проектор"); break;
    case 66: (new CBlocks     <IMyMotorAdvancedStator>(                          )).setup("Ул. Ротор"); break;
    case 67: (new CBlocks     <IMyMotorStator>        (                          )).setup("Ротор"); break;
    case 68: (new CBlocks     <IMyControlPanel>       (                          )).setup("Панель упр."); break;
    default:
    {
      Runtime.UpdateFrequency = UpdateFrequency.None;
      applyDefaultMeDisplayTexsts();
    } break;
  }
}
