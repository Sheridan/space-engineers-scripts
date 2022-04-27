// #include classes/blocks/base/functional.cs

public class CGyro : CFunctional<IMyGyro>
{
  public CGyro(CBlocksBase<IMyGyro> blocks) : base(blocks) { }

  public float power()
  {
    if(m_blocks.count() == 0) { return 0f; }
    float result = 0f;
    foreach (IMyGyro b in m_blocks) { result += b.GyroPower; }
    return result/m_blocks.count();
  }
  public void power(float val) { foreach (IMyGyro b in m_blocks) { b.GyroPower = val; } }

  public bool override()
  {
    if(m_blocks.count() == 0) { return false; }
    return m_blocks[0].GyroOverride;
  }
  public void override(bool val) { foreach (IMyGyro b in m_blocks) { b.GyroOverride = val; } }

  public float yaw()
  {
    if(m_blocks.count() == 0) { return 0f; }
    float result = 0f;
    foreach (IMyGyro b in m_blocks) { result += b.Yaw; }
    return result/m_blocks.count();
  }
  public void yaw(float val) { foreach (IMyGyro b in m_blocks) { b.Yaw = val; } }

  public float pitch()
  {
    if(m_blocks.count() == 0) { return 0f; }
    float result = 0f;
    foreach (IMyGyro b in m_blocks) { result += b.Pitch; }
    return result/m_blocks.count();
  }
  public void pitch(float val) { foreach (IMyGyro b in m_blocks) { b.Pitch = val; } }

  public float roll()
  {
    if(m_blocks.count() == 0) { return 0f; }
    float result = 0f;
    foreach (IMyGyro b in m_blocks) { result += b.Roll; }
    return result/m_blocks.count();
  }
  public void roll(float val) { foreach (IMyGyro b in m_blocks) { b.Roll = val; } }

}
