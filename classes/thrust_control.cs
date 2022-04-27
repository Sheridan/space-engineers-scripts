// #include classes/display.cs

public class CThrustControl
{
  public CThrustControl(IMyShipController controller, string lsdName)
  {
    m_lcdProducers = new CDisplay();
    m_lcdProducers.addDisplays(lsdName);
    m_controller = controller;
  }

  private IMyShipController m_controller;
}