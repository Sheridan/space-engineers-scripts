// #include classes/blocks/functional.cs

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

  public bool retract(float length, float velocity)
  {
    bool result = true;
    // float realLength = (length - pistonHeadLength * m_stackSize) / m_stackSize;
    // realLength = realLength < 0f ? 0f : realLength;
    float realLength = length / m_stackSize;
    float realVelocity = velocity / m_stackSize;
    foreach (IMyPistonBase piston in m_blocks.blocks())
    {
      switch (piston.Status)
      {
        case PistonStatus.Stopped:
        case PistonStatus.Extended:
        case PistonStatus.Extending:
        case PistonStatus.Retracted:
        {
          if (piston.CurrentPosition > realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = realLength;
            piston.MaxLimit = 10f;
            piston.Retract();
          }
        }
        break;
      }
      result = result && (piston.Status == PistonStatus.Retracted ||
                        (
                          piston.Status == PistonStatus.Retracting &&
                          checkLength(piston.CurrentPosition, realLength)
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
    foreach (IMyPistonBase piston in m_blocks.blocks())
    {
      switch (piston.Status)
      {
        case PistonStatus.Stopped:
        case PistonStatus.Retracted:
        case PistonStatus.Retracting:
        case PistonStatus.Extended:
        {
          if (piston.CurrentPosition < realLength)
          {
            piston.Velocity = realVelocity;
            piston.MinLimit = 0f;
            piston.MaxLimit = realLength;
            piston.Extend();
          }
        }
        break;
      }
      result = result && (piston.Status == PistonStatus.Extended ||
                         (
                           piston.Status == PistonStatus.Extending &&
                           checkLength(piston.CurrentPosition, realLength)
                         ));
    }
    return result;
  }

  private int m_stackSize;
  // private const float pistonHeadLength = 0.11f;
}
