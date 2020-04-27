// #include classes/display.cs

public class CWaiter
{
  public CWaiter(CDisplay display)
  {
    m_waitTo = 0;
    m_display = display;
  }

  private double getCurrentSecunds()
  {
    TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
    return timeSpan.TotalSeconds;
  }

  public void wait(double seconds)
  {
    m_display.echo($"Ждём {seconds:f2} сек.");
    m_waitTo = seconds + getCurrentSecunds();
  }

  public double leftWaitSecunds()
  {
    return m_waitTo - getCurrentSecunds();
  }

  public bool waiting()
  {
    // lcd.echo($"{waitTo}:{leftWaitSecunds()}");
    if (m_waitTo > 0 && leftWaitSecunds() > 0)
    {
      m_display.echo_last($"Ожидание. Осталось {leftWaitSecunds():f2} сек.");
      return true;
    }
    m_display.echo_last("Ожидание окончено");
    m_waitTo = 0;
    return false;
  }
  private double m_waitTo;
  private CDisplay m_display;
}
