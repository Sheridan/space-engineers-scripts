// #include classes/blocks/base/functional.cs

public class CConnector : CFunctional<IMyShipConnector>
{
  public CConnector(CBlocksBase<IMyShipConnector> blocks) : base(blocks) { }

  public bool connect(bool target = true)
  {
    foreach (IMyShipConnector b in m_blocks.blocks())
    {
      if(target) { b.Connect(); } else { b.Disconnect(); }
    }
    return checkConnected(target);
  }
  public bool disconnect() { return connect(false); }
  public bool connected () { return checkConnected(true); }

  private bool checkConnected(bool target)
  {
    bool result = true;
    foreach (IMyShipConnector b in m_blocks.blocks())
    {
      result = result &&
              (
                target ?  b.Status == MyShipConnectorStatus.Connected
                        : b.Status == MyShipConnectorStatus.Unconnected || b.Status == MyShipConnectorStatus.Connectable
              );
    }
    return result;
  }

  public bool connectable()
  {
    bool result = true;
    foreach (IMyShipConnector b in m_blocks.blocks())
    {
      result = result && b.Status == MyShipConnectorStatus.Connectable;
    }
    return result;
  }

}
