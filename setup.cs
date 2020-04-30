// #include classes/main.cs
// #include classes/blocks.cs
// #include classes/blocks_group.cs
// #include classes/blocks_typed.cs
// #include classes/block_options.cs

// public CBlockGroup<IMyShipMergeBlock> weldersMergers;
// public CBlockGroup<IMyPistonBase> weldersMergersPistons;
// public CBlockGroup<IMyShipMergeBlock> supportMergers;
// public CBlockGroup<IMyPistonBase> supportMergersPistons;
// public CBlockGroup<IMyShipMergeBlock> logisticMergers;
// public CBlockGroup<IMyPistonBase> logisticPistons;
// public CBlockGroup<IMyShipConnector> logisticConnectors;
// public CBlockGroup<IMyShipConnector> mainConnectors;
// public CBlockGroup<IMyPistonBase> mainPistons;
// public CBlockGroup<IMyShipWelder> welders;
// public CBlockGroup<IMyProjector> projectors;

// // setupBlocks<IMyProjector>("Прожектор");

public string program()
{
  return "Настройка";
}

public void main(string argument, UpdateType updateSource)
{
  (new CBlocks<IMyBatteryBlock>  ()).setup("Батарея");
  (new CBlocks<IMyRemoteControl> ()).setup("Д.У.");
  (new CBlocks<IMyOreDetector>   ()).setup("Детектор руды");
  (new CBlocks<IMyLandingGear>   ()).setup("Шасси");
  (new CBlocks<IMyShipDrill>     ()).setup("Бур");
  (new CBlocks<IMyShipGrinder>   ()).setup("Резак");
  (new CBlocks<IMyShipWelder>    ()).setup("Сварщик");
  (new CBlocks<IMyGyro>          ()).setup("Гироскоп");
  (new CBlocks<IMyCollector>     ()).setup("Коллектор");
  (new CBlocks<IMyGasGenerator>  ()).setup("H2:O2 Генератор");
  (new CBlocks<IMyShipMergeBlock>()).setup("Соединитель");
  // (new CBlocks<IMyAssembler>     ()).setup("Сборщик");
  (new CBlocks<IMyRefinery>      ()).setup("Очистительный завод");

  (new CBlocksTyped<IMyPowerProducer> ("HydrogenEngine"))        .setup("H2 Электрогенератор");
  (new CBlocksTyped<IMyPowerProducer> ("WindTurbine"))           .setup("Ветрогенератор");
  (new CBlocksTyped<IMyReflectorLight>("FrontLight"))            .setup("Прожектор");
  (new CBlocksTyped<IMyThrust>        ("LargeAtmosphericThrust")).setup("Б.А.У.");
  (new CBlocksTyped<IMyThrust>        ("SmallAtmosphericThrust")).setup("А.У.");
  (new CBlocksTyped<IMyThrust>        ("LargeHydrogenThrust"))   .setup("Б.В.У.");
  (new CBlocksTyped<IMyThrust>        ("SmallHydrogenThrust"))   .setup("В.У.");
  (new CBlocksTyped<IMyGasTank>       ("OxygenTankSmall"))       .setup("Бак O2", false, true);
  (new CBlocksTyped<IMyGasTank>       ("HydrogenTank"))          .setup("Б.Бак H2", false, true);
  (new CBlocksTyped<IMyGasTank>       ("HydrogenTankSmall"))     .setup("Бак H2", false, true);
  (new CBlocksTyped<IMyCargoContainer>("SmallContainer"))        .setup("МК", false, true);
  (new CBlocksTyped<IMyCargoContainer>("MediumContainer"))       .setup("СК", false, true);
  (new CBlocksTyped<IMyCargoContainer>("LargeContainer"))        .setup("БК", false, true);
  (new CBlocksTyped<IMyUpgradeModule> ("ProductivityModule"))    .setup("Модуль Продуктивности");
  (new CBlocksTyped<IMyUpgradeModule> ("EffectivenessModule"))   .setup("Модуль Эффективности");
  (new CBlocksTyped<IMyUpgradeModule> ("EnergyModule"))          .setup("Модуль Энергоэффективности");
}
