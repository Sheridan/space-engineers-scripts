// #include classes/blocks/base/functional.cs

public class CBattery : CFunctional<IMyBatteryBlock>
{
  public CBattery(CBlocksBase<IMyBatteryBlock> blocks) : base(blocks) { }

  public bool setChargeMode(ChargeMode mode)
  {
    bool result = true;
    foreach (IMyBatteryBlock b in m_blocks)
    {
      if(b.ChargeMode != mode) { b.ChargeMode = mode; }
      result = result && b.ChargeMode == mode;
    }
    return result;
  }

  public bool recharge  () { return setChargeMode(ChargeMode.Recharge ); }
  public bool discharge () { return setChargeMode(ChargeMode.Discharge); }
  public bool autocharge() { return setChargeMode(ChargeMode.Auto     ); }

}
