// #include classes/blocks/functional.cs

public class CConnector : CFunctional<IMyShipConnector>
{
  public CConnector(CBlocksBase<IMyShipConnector> blocks) : base(blocks) { }

  public bool connect(bool enabled = true)
  {
    bool result = true;
    foreach (IMyShipConnector connector in m_blocks.blocks())
    {
      if(enabled) { connector.Connect(); } else { connector.Disconnect(); }
      result = result &&
              (
               enabled ? connector.Status == MyShipConnectorStatus.Connected
                       : connector.Status == MyShipConnectorStatus.Unconnected
              );
    }
    return result;
  }
  public bool disconnect() { return connect(false); }

}
