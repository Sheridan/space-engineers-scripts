// #include classes/main.cs
// #include classes/block_group.cs
// #include classes/display.cs

public Vector3D getGravity() { return cockpit.GetNaturalGravity(); }
public Vector3D getPosition() { return cockpit.GetPosition(); }

CBlockGroup<IMyGyro> gyroscopes;
CTextSurface lcd;
IMyCockpit cockpit;

public string program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
  cockpit = self.GridTerminalSystem.GetBlockWithName("[Жук] Кокпит") as IMyCockpit;
  lcd = new CTextSurface();
  lcd.setSurface(cockpit.GetSurface(3), 2f, 7, 30);
  gyroscopes = new CBlockGroup<IMyGyro>("[Жук] Гироскопы автогоризонта", "Гироскопы автогоризонта");
  // lcd = new CBlockStatusDisplay("[Крот] Дисплей состояния");
  return "Атоматический горизонт";
}

public double fullAngle(Vector3D v1, Vector3D v2)
{
  Vector3D v1Norm = Vector3D.Normalize(v1);
  Vector3D v2Norm = Vector3D.Normalize(v2);
  return Math.Acos
  (
    v1Norm.Dot(v2Norm)/(v1Norm.Length()*v2Norm.Length())
    // (v1.X*v2.X + v1.Y*v2.Y + v1.Z*v2.Z)
    // /
    // (
    //   Math.Sqrt(Math.Pow(v1.X, 2) + Math.Pow(v1.Y, 2) + Math.Pow(v1.Z, 2))
    //   *
    //   Math.Sqrt(Math.Pow(v2.X, 2) + Math.Pow(v2.Y, 2) + Math.Pow(v2.Z, 2))
    // )
  ) * 180 / (float) Math.PI;
}

public void echoVector(Vector3D vector, string name, int pos)
{
  lcd.echo_at($"[{name}] X:{vector.X:f2},Y:{vector.Y:f2},Z:{vector.Z:f2}", pos);
}

public void main(string argument, UpdateType updateSource)
{
  // Vector3D gravity = getGravity();
  echoVector(getGravity(), "G", 0);
  echoVector(getPosition(), "P", 1);
  double angle = fullAngle(getGravity(), getPosition());
  lcd.echo_at($"{angle:f8}", 2);
}
