// #include classes/blocks/functional.cs

public class CRotor : CFunctional<IMyMotorStator>
{
  public CRotor(CBlocksBase<IMyMotorStator> blocks) : base(blocks)
  {
    m_reversed = false;
  }

  public void rotate(float rpm = 1f)
  {
    foreach (IMyMotorStator block in m_blocks.blocks())
    {
      block.TargetVelocityRPM = rpm;
    }
  }

  public void stop()
  {
    foreach (IMyMotorStator block in m_blocks.blocks())
    {
      block.TargetVelocityRPM = 0f;
    }
  }

  public void reverse()
  {
    foreach (IMyMotorStator block in m_blocks.blocks())
    {
      block.GetActionWithName("Reverse").Apply(block);
    }
    m_reversed = !m_reversed;
  }

  public float angle()
  {
    float result = 0f;
    foreach (IMyMotorStator block in m_blocks.blocks())
    {
      result += block.Angle;
    }
    float agl = (result/m_blocks.count()) * (180f/3.1415926f);
    return m_reversed ? 360f - agl : agl;
  }

  public void setLimit(float lmin = float.MinValue, float lmax = float.MaxValue)
  {
    foreach (IMyMotorStator block in m_blocks.blocks())
    {
      block.LowerLimitDeg = lmin;
      block.UpperLimitRad = lmax;
    }
  }

  private bool m_reversed;
}
