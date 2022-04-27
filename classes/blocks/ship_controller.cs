// #include classes/display.cs
// #include helpers/human.cs
// #include classes/blocks/gyro.cs
// #include classes/blocks/thrust.cs

public class CShipController
{
  public CShipController(string ctrlName, string lcdName)
  {
    self.GridTerminalSystem.GetBlockWithName(ctrlName) as IMyShipController;
    m_lcd = new CDisplay();
    m_lcd.addDisplays(lcdName);
    m_gyro = CGyro(new CBlocks<IMyGyro>());
  }

  public float shipMass() { return m_ctrl.CalculateShipMass().TotalMass; }
  public double shipSpeed() { return m_ctrl.GetShipSpeed(); }
  public bool inGravity() { return m_controller.GetTotalGravity().Length() > 0; }

  IMyShipController m_ctrl;
  CGyro m_gyro;
  CThrust m_thrust;
  CDisplay m_lcd;
}
