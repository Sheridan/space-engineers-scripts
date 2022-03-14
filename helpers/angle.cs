public class CAngle
{
  public CAngle(float angle) { m_angle = checkBoards(angle); }
  public float angle() { return m_angle; }
  private static float checkBoards(float angle)
  {
    if (angle == 360)              { return 0; }
    if (angle  > 360 || angle < 0) { return Math.Abs(angle % 360); }
    return angle;
  }
  public static CAngle fromRad(float rad) { return new CAngle(rad * 180 / (float)Math.PI); }
  public float toRad() { return m_angle * (float)Math.PI/180; }

  public static CAngle operator +(CAngle a, CAngle b) { return new CAngle(a.angle() + b.angle()); }
  public static CAngle operator -(CAngle a, CAngle b) { return new CAngle(a.angle() - b.angle()); }
  public static CAngle operator *(CAngle a, CAngle b) { return new CAngle(a.angle() * b.angle()); }
  public static CAngle operator /(CAngle a, CAngle b) { return new CAngle(a.angle() / b.angle()); }
  public static CAngle operator +(CAngle a,  float b) { return new CAngle(a.angle() + checkBoards(b)); }
  public static CAngle operator -(CAngle a,  float b) { return new CAngle(a.angle() - checkBoards(b)); }
  public static CAngle operator *(CAngle a,  float b) { return new CAngle(a.angle() * checkBoards(b)); }
  public static CAngle operator /(CAngle a,  float b) { return new CAngle(a.angle() / checkBoards(b)); }
  public static CAngle operator ++(CAngle a) { return new CAngle(a.angle()+1f); }
  public static CAngle operator --(CAngle a) { return new CAngle(a.angle()-1f); }

  public static bool operator false(CAngle a) { return a.angle() == 0f; }
  public static bool operator true (CAngle a) { return a.angle()  > 0f; }
  public static bool operator ==(CAngle a, CAngle b) { return a.angle() == b.angle(); }
  public static bool operator !=(CAngle a, CAngle b) { return a.angle() != b.angle(); }
  public static bool operator >=(CAngle a, CAngle b) { return a.angle() >= b.angle(); }
  public static bool operator <=(CAngle a, CAngle b) { return a.angle() <= b.angle(); }
  public static bool operator  >(CAngle a, CAngle b) { return a.angle()  > b.angle(); }
  public static bool operator  <(CAngle a, CAngle b) { return a.angle()  < b.angle(); }
  public static bool operator ==(CAngle a,  float b) { return a.angle() == checkBoards(b); }
  public static bool operator !=(CAngle a,  float b) { return a.angle() != checkBoards(b); }
  public static bool operator >=(CAngle a,  float b) { return a.angle() >= checkBoards(b); }
  public static bool operator <=(CAngle a,  float b) { return a.angle() <= checkBoards(b); }
  public static bool operator  >(CAngle a,  float b) { return a.angle()  > checkBoards(b); }
  public static bool operator  <(CAngle a,  float b) { return a.angle()  < checkBoards(b); }

  public override bool Equals(object obj)
  {
    if (obj == null) { return false; }
    CAngle a = obj as CAngle; if (a as CAngle == null) { return false; }
    return a.angle() == m_angle;
  }

  public override int GetHashCode() { return (int)(m_angle*100000f); }
  public override string ToString() { return $"{m_angle:f2}°"      ; }

  private float m_angle;
}
