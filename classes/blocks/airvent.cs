// #include classes/blocks/base/functional.cs

public class CAirVent : CFunctional<IMyAirVent>
{
  public CAirVent(CBlocksBase<IMyAirVent> blocks) : base(blocks) { }

  public bool canPressurize()
  {
    bool result = true;
    foreach (IMyAirVent b in m_blocks) { result = result && b.CanPressurize; }
    return result;
  }

  public bool isPressurized()
  {
    bool result = true;
    foreach (IMyAirVent b in m_blocks) { result = result && b.Status == VentStatus.Pressurized; }
    return result;
  }

  public bool isDepressurized()
  {
    bool result = true;
    foreach (IMyAirVent b in m_blocks) { result = result && b.Status == VentStatus.Depressurized; }
    return result;
  }

  public void pressurize()
  {
    foreach (IMyAirVent b in m_blocks) { b.Depressurize = false; }
  }

  public void depressurize()
  {
    foreach (IMyAirVent b in m_blocks) { b.Depressurize = true; }
  }

  public float oxygenLevel()
  {
    float result = 0f;
    foreach (IMyAirVent b in m_blocks) { result += b.GetOxygenLevel(); }
    return (result/m_blocks.count())*100f;
  }
}
