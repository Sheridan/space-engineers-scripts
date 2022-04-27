// #include classes/blocks/base/blocks_base.cs

public class CThrust : CFunctional<IMyThrust>
{
  public CTank(CBlocksBase<IMyThrust> blocks) : base(blocks) { }

  public float maxThrust()
  {
    float result = 0f;
    foreach (IMyThrust b in m_blocks) { result += b.MaxThrust; }
    return result;
  }
  public float maxEffectiveThrust()
  {
    float result = 0f;
    foreach (IMyThrust b in m_blocks) { result += b.MaxEffectiveThrust; }
    return result;
  }
  public float currentThrust()
  {
    float result = 0f;
    foreach (IMyThrust b in m_blocks) { result += b.CurrentThrust; }
    return result;
  }
  public float ovverided()
  {
    foreach (IMyThrust b in m_blocks) { if(b.ThrustOverride > float.MinValue) { return true; } }
    return false;
  }
  public float thrustOverride()
  {
    float result = 0f;
    foreach (IMyThrust b in m_blocks) { result += b.ThrustOverride; }
    return result;
  }
  public void thrustOverride(float newtons)
  {
    float result = 0f;
    foreach (IMyThrust b in m_blocks) { result += b.ThrustOverride; }
    return result;
  }
}