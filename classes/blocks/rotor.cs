// #include classes/blocks/base/functional.cs

public class CRotor : CFunctional<IMyMotorStator>
{
  public CRotor(CBlocksBase<IMyMotorStator> blocks) : base(blocks)
  {
    m_reversed = false;
  }

  public void rotate(float rpm = 1f)
  {
    foreach (IMyMotorStator b in m_blocks.blocks())
    {
      b.TargetVelocityRPM = rpm;
    }
  }

  public void stop()
  {
    foreach (IMyMotorStator b in m_blocks.blocks())
    {
      b.TargetVelocityRPM = 0f;
    }
  }

  public void reverse()
  {
    foreach (IMyMotorStator b in m_blocks.blocks())
    {
      b.GetActionWithName("Reverse").Apply(b);
    }
    m_reversed = !m_reversed;
  }

  public float angle()
  {
    float result = 0f;
    foreach (IMyMotorStator b in m_blocks.blocks())
    {
      result += b.Angle;
    }
    float agl = (result/m_blocks.count()) * (180f/3.1415926f);
    return m_reversed ? 360f - agl : agl;
  }

  public void setLimit(float lmin = float.MinValue, float lmax = float.MaxValue)
  {
    foreach (IMyMotorStator b in m_blocks.blocks())
    {
      b.LowerLimitDeg = lmin;
      b.UpperLimitDeg = lmax;
    }
  }

  private bool m_reversed;
}
