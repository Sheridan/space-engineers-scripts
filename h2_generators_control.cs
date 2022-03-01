// #include classes/main.cs
// #include classes/blocks.cs
// #include classes/blocks_typed.cs
// #include classes/blocks/functional.cs

// public CBlocksTyped<IMyGasTank> h2tanks;
// public CBlocksTyped<IMyPowerProducer> h2Engines;
// public CBlocks<IMyGasGenerator> gasGenerators;
public CFunctional<IMyPowerProducer> h2Engines;
// public CFunctional<IMyGasGenerator> gasGenerators;
bool generationOn;

public string program()
{
  // Runtime.UpdateFrequency = UpdateFrequency.Update100;
  // h2Engines = new CBlocksTyped<IMyPowerProducer>("HydrogenEngine");
  // h2tanks = new CBlocksTyped<IMyGasTank>("HydrogenTank", "H2");
  // gasGenerators = new CBlocks<IMyGasGenerator>();
  // gasGenerators = new CFunctional<IMyGasGenerator>(new CBlocks<IMyGasGenerator>());
  h2Engines = new CFunctional<IMyPowerProducer>(new CBlocksTyped<IMyPowerProducer>("HydrogenEngine"));
  generationOn = false;
  switchGenerate();
  return "Управление водородными генераторами";
}

public void switchGenerate()
{
  // gasGenerators.enable(generationOn);
  h2Engines.enable(generationOn);
  generationOn = !generationOn;
}

public void main(string argument, UpdateType updateSource)
{
  if (argument == "generate")
  {
    switchGenerate();
  }

}
