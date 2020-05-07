// #include classes/main.cs
// #include classes/blocks.cs
// #include classes/blocks_group.cs
// #include classes/blocks_typed.cs
// #include classes/block_options.cs

// public CBlockGroup<IMyPistonBase> weldersMergersPistons;
// public CBlockGroup<IMyShipConnector> logisticConnectors;
// public CBlockGroup<IMyProjector> projectors;

public string program() { return "Настройка структуры"; }

public void main(string argument, UpdateType updateSource)
{
  (new CBlocks<IMyBatteryBlock>      ()).setup("Батарея");
  (new CBlocks<IMySolarPanel>        ()).setup("С.Батарея");
  (new CBlocks<IMyRemoteControl>     ()).setup("Д.У.");
  (new CBlocks<IMyOreDetector>       ()).setup("Детектор руды");
  (new CBlocks<IMyLandingGear>       ()).setup("Шасси");
  (new CBlocks<IMyShipDrill>         ()).setup("Бур");
  (new CBlocks<IMyShipGrinder>       ()).setup("Резак");
  (new CBlocks<IMyShipWelder>        ()).setup("Сварщик");
  (new CBlocks<IMyGyro>              ()).setup("Гироскоп");
  (new CBlocks<IMyCollector>         ()).setup("Коллектор");
  (new CBlocks<IMyGasGenerator>      ()).setup("H2:O2 Генератор");
  (new CBlocks<IMyShipMergeBlock>    ()).setup("Соединитель");
  // (new CBlocks<IMyAssembler>         ()).setup("Сборщик");
  (new CBlocks<IMyRefinery>          ()).setup("Очистительный завод");
  (new CBlocks<IMyMedicalRoom>       ()).setup("Медпост");
  (new CBlocks<IMySmallGatlingGun>   ()).setup("М.Пушка");
  (new CBlocks<IMyLargeGatlingTurret>()).setup("Б.Пушка");
  (new CBlocks<IMyDoor>              ()).setup("Дверь");
  (new CBlocks<IMyAirVent>           ()).setup("Вентиляция");

  (new CBlocksTyped<IMyPowerProducer> ("HydrogenEngine"))        .setup("H2 Электрогенератор");
  (new CBlocksTyped<IMyPowerProducer> ("WindTurbine"))           .setup("Ветрогенератор");
  (new CBlocksTyped<IMyThrust>        ("LargeAtmosphericThrust")).setup("Б.А.У.");
  (new CBlocksTyped<IMyThrust>        ("SmallAtmosphericThrust")).setup("А.У.");
  (new CBlocksTyped<IMyThrust>        ("LargeHydrogenThrust"))   .setup("Б.В.У.");
  (new CBlocksTyped<IMyThrust>        ("SmallHydrogenThrust"))   .setup("В.У.");
  (new CBlocksTyped<IMyGasTank>       ("OxygenTankSmall"))       .setup("Бак O2", false, true);
  (new CBlocks<IMyOxygenTank>                                 ()).setup("Б.Бак O2", false, true);
  (new CBlocksTyped<IMyGasTank>       ("HydrogenTankSmall"))     .setup("Бак H2", false, true);
  (new CBlocksTyped<IMyGasTank>       ("HydrogenTank"))          .setup("Б.Бак H2", false, true);
  (new CBlocksTyped<IMyCargoContainer>("SmallContainer"))        .setup("МК", false, true);
  (new CBlocksTyped<IMyCargoContainer>("MediumContainer"))       .setup("СК", false, true);
  (new CBlocksTyped<IMyCargoContainer>("LargeContainer"))        .setup("БК", false, true);
  (new CBlocksTyped<IMyUpgradeModule> ("ProductivityModule"))    .setup("Модуль Продуктивности");
  (new CBlocksTyped<IMyUpgradeModule> ("EffectivenessModule"))   .setup("Модуль Эффективности");
  (new CBlocksTyped<IMyUpgradeModule> ("EnergyModule"))          .setup("Модуль Энергоэффективности");

  (new CBlocksTyped<IMyInteriorLight> ("SmallLight"))            .setup("Лампа");
  (new CBlocksTyped<IMyInteriorLight> ("Light_1corner"))         .setup("Угл. Лампа");
  (new CBlocksTyped<IMyInteriorLight> ("Light_2corner"))         .setup("2хУгл. Лампа");
  (new CBlocksTyped<IMyReflectorLight>("FrontLight"))            .setup("Прожектор");
  (new CBlocksTyped<IMyReflectorLight>("RotatingLight"))         .setup("Вр. прожектор");

  (new CBlocksTyped<IMyMotorSuspension>("Suspension1x1"))        .setup("Колесо 1x1 правое");
  (new CBlocksTyped<IMyMotorSuspension>("Suspension3x3"))        .setup("Колесо 3x3 правое");
  (new CBlocksTyped<IMyMotorSuspension>("Suspension5x5"))        .setup("Колесо 5x5 правое");
  (new CBlocksTyped<IMyMotorSuspension>("Suspension1x1mirrored")).setup("Колесо 1x1 левое");
  (new CBlocksTyped<IMyMotorSuspension>("Suspension3x3mirrored")).setup("Колесо 3x3 левое");
  (new CBlocksTyped<IMyMotorSuspension>("Suspension5x5mirrored")).setup("Колесо 5x5 левое");
}
