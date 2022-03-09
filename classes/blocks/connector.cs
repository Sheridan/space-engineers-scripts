// #include classes/blocks/functional.cs

public class CConnector : CFunctional<IMyShipConnector>
{
  public CConnector(CBlocksBase<IMyShipConnector> blocks) : base(blocks) { }

  public bool connect(bool target = true)
  {
    foreach (IMyShipConnector connector in m_blocks.blocks())
    {
      if(target) { connector.Connect(); } else { connector.Disconnect(); }
    }
    return checkConnected(target);
  }
  public bool disconnect() { return connect(false); }
  public bool connected () { return checkConnected(true); }

  private bool checkConnected(bool target)
  {
    bool result = true;
    foreach (IMyShipConnector connector in m_blocks.blocks())
    {
      result = result &&
              (
                target ?  connector.Status == MyShipConnectorStatus.Connected
                        : connector.Status == MyShipConnectorStatus.Unconnected || connector.Status == MyShipConnectorStatus.Connectable
              );
    }
    return result;
  }

  public bool connectable()
  {
    bool result = true;
    foreach (IMyShipConnector connector in m_blocks.blocks())
    {
      result = result && connector.Status == MyShipConnectorStatus.Connectable;
    }
    return result;
  }

}
