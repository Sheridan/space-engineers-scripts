// #include classes/blocks/functional.cs

public class CBattery : CFunctional<IMyBatteryBlock>
{
  public CBattery(CBlocksBase<IMyBatteryBlock> blocks) : base(blocks) { }

  public bool setChargeMode(ChargeMode mode)
  {
    bool result = true;
    foreach (IMyBatteryBlock battery in m_blocks.blocks())
    {
      if(battery.ChargeMode != mode) { battery.ChargeMode = mode; }
      result = result && battery.ChargeMode == mode;
    }
    return result;
  }

  public bool recharge  () { return setChargeMode(ChargeMode.Recharge ); }
  public bool discharge () { return setChargeMode(ChargeMode.Discharge); }
  public bool autocharge() { return setChargeMode(ChargeMode.Auto     ); }

}
