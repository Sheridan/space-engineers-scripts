// #include helpers/math.cs
// #include classes/blocks/base/blocks_base.cs

// roll - крен
// pitch - тангаж
// yaw - рысканье
//
// translation - перемещение
// scale - масштабирование

public class CAutoHorizont
{
  public CAutoHorizont(IMyShipController controller, CBlocksBase<IMyGyro> gyroscopes)
  {
    m_controller = controller;
    m_gyroscopes = gyroscopes;
  }

  public void step(float yaw)
  {
    checkInGravity();
    if (!enabled()) { return; }
      Vector3D normGravity = Vector3D.Normalize(m_controller.GetNaturalGravity());
      applyGyroOverride(yaw,
                       (float)normGravity.Dot(m_controller.WorldMatrix.Left),
                       (float)normGravity.Dot(m_controller.WorldMatrix.Forward));
  }

  public bool enabled() { return m_gyroscopes.blocks()[0].GyroOverride; }

  public void disable()
  {
    if (!enabled()) { return; }
    foreach (IMyGyro gyroscope in m_gyroscopes.blocks())
    {
      gyroscope.Yaw = 0;
      gyroscope.Roll = 0;
      gyroscope.Pitch = 0;
      gyroscope.GyroOverride = false;
    }
  }

  public void enable()
  {
    if (enabled() || !inGravity()) { return; }
    foreach (IMyGyro gyroscope in m_gyroscopes.blocks())
    {
      gyroscope.Yaw = 0;
      gyroscope.Roll = 0;
      gyroscope.Pitch = 0;
      gyroscope.GyroOverride = true;
    }
  }

  private void applyGyroOverride(double yaw, double roll, double pitch)
  {
    Vector3D relRotVector = Vector3D.TransformNormal(new Vector3D(-pitch, yaw, roll),
                                                            m_controller.WorldMatrix);
    foreach (IMyGyro gyroscope in m_gyroscopes.blocks())
    {
      Vector3D transRotVector = Vector3D.TransformNormal(relRotVector, Matrix.Transpose(gyroscope.WorldMatrix));
      gyroscope.Yaw = (float)transRotVector.Y;
      gyroscope.Roll = (float)transRotVector.Z;
      gyroscope.Pitch = (float)transRotVector.X;
    }
  }

  private bool inGravity()
  {
    return m_controller.GetNaturalGravity().Length() > 0;
  }

  private void checkInGravity()
  {
         if(  enabled() && !inGravity()) { disable();}
    else if (!enabled() &&  inGravity()) { enable ();}
  }

  public void debugAH()
  {
    Vector3D normGravity = Vector3D.Normalize(m_controller.GetNaturalGravity());
    debug($"Ctrl: {m_controller.DisplayNameText}");
    debug($"Left: {normGravity.Dot(m_controller.WorldMatrix.Left)}");
    debug($"Forward: {normGravity.Dot(m_controller.WorldMatrix.Forward)}");
  }

  private IMyShipController m_controller;
  private CBlocksBase<IMyGyro> m_gyroscopes;
}
