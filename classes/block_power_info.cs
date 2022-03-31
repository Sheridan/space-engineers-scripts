class CBlockUpgrades
{
  public CBlockUpgrades(IMyUpgradableBlock upBlock)
  {
    Dictionary<string, float> upgrades = new Dictionary<string, float>();
    upBlock.GetUpgrades(out upgrades);
    Effectiveness   = upgrades.ContainsKey("Effectiveness"  ) ? upgrades["Effectiveness"  ] : 0;
    Productivity    = upgrades.ContainsKey("Productivity"   ) ? upgrades["Productivity"   ] : 0;
    PowerEfficiency = upgrades.ContainsKey("PowerEfficiency") ? upgrades["PowerEfficiency"] : 0;
  }

  public float calcPowerUse(float power)
  {
    return (power+((power/2)*(Productivity*2)))/(PowerEfficiency > 1 ? (float)Math.Pow(1.2228445f,PowerEfficiency) : 1);
  }

  // public CBlockPowerInfo recalculatePowerUse(CBlockPowerInfo bpInfo)
  // {
  //   CPowerInfo power = bpInfo.getPower(EPowerDirection.Consume);
  //   // power.current    = calcPowerUse(power.current);
  //   power.max        = calcPowerUse(power.max    );
  //   bpInfo.setPower(EPowerDirection.Consume, power);
  //   return bpInfo;
  // }

  // public string toString()
  // {
  //   return $"Eff:{Effectiveness};Prod:{Productivity};Pow:{PowerEfficiency};";
  // }

  private float Effectiveness;
  private float Productivity;
  private float PowerEfficiency;
}

public class CBlockPowerInfo
{
  public CBlockPowerInfo(IMyTerminalBlock block)
  {
    m_block = block;
    m_blockSinkComponent = m_block.Components.Get<MyResourceSinkComponent>();
  }

  public bool canProduce() { return m_block is IMyPowerProducer; }
  public bool canConsume()
  {
    return m_blockSinkComponent != null && m_blockSinkComponent.IsPoweredByType(Electricity);
  }

  public float currentProduce()
  {
    if(canProduce()) { return (m_block as IMyPowerProducer).CurrentOutput*1000000f; }
    return 0f;
  }

  public float maxProduce()
  {
    if (canProduce()) { return (m_block as IMyPowerProducer).MaxOutput*1000000f; }
    return 0f;
  }

  public float currentConsume()
  {
    if (canConsume())
    {
      float result = m_blockSinkComponent.CurrentInputByType(Electricity);
      // if (m_block is IMyAssembler || m_block is IMyRefinery)
      // {
      //   CBlockUpgrades upgrades = new CBlockUpgrades(m_block as IMyUpgradableBlock);
      //   upgrades.calcPowerUse(result);
      // }
      return result * 1000000f;
    }
    return 0f;
  }

  public float maxConsume()
  {
    if (canConsume())
    {
      float result = m_blockSinkComponent.MaxRequiredInputByType(Electricity);
      if (m_block is IMyAssembler || m_block is IMyRefinery)
      {
        CBlockUpgrades upgrades = new CBlockUpgrades(m_block as IMyUpgradableBlock);
        upgrades.calcPowerUse(result);
      }
      return result * 1000000f;
    }
    return 0f;
  }

  MyResourceSinkComponent m_blockSinkComponent;
  IMyTerminalBlock m_block;
  private static readonly MyDefinitionId Electricity = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Electricity");
}
