// #include classes/blocks/base/functional.cs

public class CPiston : CFunctional<IMyPistonBase>
{
  public CPiston(CBlocksBase<IMyPistonBase> blocks, int pistonsInStack = 1) : base(blocks)
  {
    m_stackSize = pistonsInStack;
  }

  private bool checkLength(float currentPos, float targetPos, float sensetivity = 0.2f)
  {
    return currentPos <= targetPos + sensetivity && currentPos >= targetPos - sensetivity;
  }

  public float currentLength()
  {
    float l = 0;
    foreach (IMyPistonBase b in m_blocks)
    {
      l += b.CurrentPosition;
    }
    return l/m_blocks.count()/m_stackSize;
  }

  public bool retract(float length, float velocity)
  {
    bool result = true;
    // float realLength = (length - pistonHeadLength * m_stackSize) / m_stackSize;
    // realLength = realLength < 0f ? 0f : realLength;
    float realLength = length / m_stackSize;
    float realVelocity = velocity / m_stackSize;
    foreach (IMyPistonBase b in m_blocks)
    {
      switch (b.Status)
      {
        case PistonStatus.Stopped:
        case PistonStatus.Extended:
        case PistonStatus.Extending:
        case PistonStatus.Retracted:
        {
          if (b.CurrentPosition > realLength)
          {
            b.Velocity = realVelocity;
            b.MinLimit = realLength;
            b.MaxLimit = 10f;
            b.Retract();
          }
        }
        break;
      }
      result = result && (b.Status == PistonStatus.Retracted ||
                        (
                          b.Status == PistonStatus.Retracting &&
                          checkLength(b.CurrentPosition, realLength)
                        ));
    }
    return result;
  }

  public bool expand(float length, float velocity)
  {
    bool result = true;
    // float realLength = (length - pistonHeadLength * m_stackSize) / m_stackSize;
    float realLength = length / m_stackSize;
    float realVelocity = velocity / m_stackSize;
    foreach (IMyPistonBase b in m_blocks)
    {
      switch (b.Status)
      {
        case PistonStatus.Stopped:
        case PistonStatus.Retracted:
        case PistonStatus.Retracting:
        case PistonStatus.Extended:
        {
          if (b.CurrentPosition < realLength)
          {
            b.Velocity = realVelocity;
            b.MinLimit = 0f;
            b.MaxLimit = realLength;
            b.Extend();
          }
        }
        break;
      }
      result = result && (b.Status == PistonStatus.Extended ||
                         (
                           b.Status == PistonStatus.Extending &&
                           checkLength(b.CurrentPosition, realLength)
                         ));
    }
    return result;
  }

  public bool expandRelative (float length, float velocity) { return expand (currentLength() + length, velocity); }
  public bool retractRelative(float length, float velocity) { return retract(currentLength() - length, velocity); }

  private int m_stackSize;
  // private const float pistonHeadLength = 0.11f;
}
