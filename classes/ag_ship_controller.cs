// #include classes/blocks/base/blocks_base.cs
// #include classes/auto_horizont.cs

public class CAGShipController
{
  public CAGShipController(IMyShipController controller, CBlocksBase<IMyGyro> gyroscopes)
  {
    m_controller = controller;
    m_autoHorizont = new CAutoHorizont(m_controller, gyroscopes);
  }

  public CAutoHorizont autoHorizont() { return m_autoHorizont; }

  private CAutoHorizont m_autoHorizont;
  private IMyShipController m_controller;
}
