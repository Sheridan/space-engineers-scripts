// #include helpers/math.cs
// #include classes/blocks_group.cs

// roll - крен
// pitch - тангаж
// yaw - рысканье
//
// translation - перемещение
// scale - масштабирование

class CShipController
{
  public CShipController(IMyShipController controller)
  {
    m_controller = controller;
    m_autoHorizontGyroscopes = null;
  }

  public CShipController(string name)
  {
    m_controller = self.GridTerminalSystem.GetBlockWithName(name) as IMyShipController;
    m_autoHorizontGyroscopes = null;
  }

  public void enableAutoHorizont(CBlockGroup<IMyGyro> gyroscopes)
  {
    if (autoHorizontIsEnabled()) { return; }
    m_autoHorizontGyroscopes = gyroscopes;
    foreach (IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks())
    {
      gyroscope.GyroOverride = true;
    }
  }

  public void disableAutoHorizont()
  {
    if (!autoHorizontIsEnabled()) { return; }
    foreach (IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks())
    {
      gyroscope.Yaw = 0;
      gyroscope.Roll = 0;
      gyroscope.Pitch = 0;
      gyroscope.GyroOverride = false;
    }
    m_autoHorizontGyroscopes = null;
  }

  public void autoHorizont(float yaw)
  {
    if(!autoHorizontIsEnabled()) { return; }
    Vector3D normGravity = Vector3D.Normalize(m_controller.GetNaturalGravity());
    foreach(IMyGyro gyroscope in m_autoHorizontGyroscopes.blocks())
    {
      gyroscope.Yaw   = yaw;
      gyroscope.Roll  = (float)-normGravity.Dot(m_controller.WorldMatrix.Left);
      gyroscope.Pitch = (float) normGravity.Dot(m_controller.WorldMatrix.Forward);
    }
  }

  public bool autoHorizontIsEnabled() { return m_autoHorizontGyroscopes != null; }

  // private double calcAngle(Vector3D shipVector)
  // {
  //   return angleBetweenVectors(shipVector, m_controller.GetNaturalGravity());
  // }

  // public void test()
  // {
  //   MatrixD g2w = gridTWorldTransformMatrix(m_controller.CubeGrid);
  //   // Vector3D origin = m_controller.CubeGrid.GridIntegerToWorld(Vector3I.Zero);
  //   debug($"{origin.X:f4}:{origin.Y:f4}:{origin.Z:f4}");
  //   // Quaternion q;
  //   // m_controller.Orientation.GetQuaternion(out q);
  //   // debug($"{q.X}:{q.Y}:{q.Z}:{q.W}");
  // }

  // private double calcAxisAngle(Vector3D shipAxis)
  // { return 0; }
  // {
  //   // Matrix m;
  //   // m_controller.Orientation.GetMatrix(out m);
  //   // Vector3D ra = Vector3D.Rotate(shipAxis, m);
  //   // Quaternion q;
  //   // m_controller.Orientation.GetQuaternion(out q);
  //   // Vector3 v;
  //   // float a;
  //   // Quaternion.GetForward(ref q, out v);
  //   // q.GetAxisAngle(out v, out a);

  //   return angleBetweenVectors(shipAxis, v);
  //   // return a;
  // }


  // public double pitchAxisAngle()
  // {
  //   return angleBetweenVectors(m_controller.CubeGrid.WorldMatrix.Forward, Vector3D.Up);
  //   // return calcAxisAngle(m_controller.WorldMatrix.Up);
  // }
  // public double rollAxisAngle() { return calcAxisAngle(m_controller.WorldMatrix.Forward); }
  // public double yawAxisAngle() { return calcAxisAngle(m_controller.WorldMatrix.Left); }

  // private double calcAngle(Vector3D shipVector)
  // {
  //   return angleBetweenVectors(shipVector, m_controller.GetNaturalGravity());

    // Matrix m;
    // m_controller.Orientation.GetMatrix(out m);
    // Vector3D gravity = Vector3D.Rotate(m_controller.GetNaturalGravity(), m);
    // return angleBetweenVectors(m_controller.GetNaturalGravity(), gravity);

    // Vector3D position = m_controller.WorldMatrix.GetOrientation().Up;

    // Matrix m;
    // m_controller.Orientation.GetMatrix(out m);
    // Vector3D orientation = m.Backward ;
    // Vector3D position = m_controller.GetPosition();
    // Vector3D gravity = m_controller.GetNaturalGravity();
    // Vector3D projectedPosition = Vector3D.ProjectOnPlane(ref position, ref shipVector);
    // Vector3D projectedGravity = Vector3D.ProjectOnPlane(ref gravity, ref shipVector);
    // return angleBetweenVectors(projectedPosition, projectedGravity);

    // Vector3D position = m_controller.GetPosition();
    // return angleBetweenVectors(shipVector, m_controller.GetNaturalGravity());

    // Vector3D gravity = Vector3D.TransformNormal(m_controller.GetNaturalGravity(), m_controller.Orientation);
    // Vector3D orientation = Vector3D.TransformNormal(shipVector, m_controller.Orientation);
    // return angleBetweenVectors(orientation, gravity);
    // Vector3D localTargetVector = Vector3D.TransformNormal(gravity, MatrixD.Transpose(m_controller.WorldMatrix));
  // }

  // public double pitchAngle() { return calcAngle(m_controller.WorldMatrix.Forward); }
  // public double rollAngle() { return calcAngle(m_controller.WorldMatrix.Down); }
  // public double yawAngle() { return calcAngle(m_controller.WorldMatrix.Left); }

  private IMyShipController m_controller;
  CBlockGroup<IMyGyro> m_autoHorizontGyroscopes;
  // bool m_autoHorizontEnabled;
}
