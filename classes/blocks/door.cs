// #include classes/blocks/base/functional.cs

public class CDoor : CFunctional<IMyDoor>
{
  public CDoor(CBlocksBase<IMyDoor> blocks) : base(blocks) { }

  public bool isOpen()
  {
    bool result = true;
    foreach (IMyDoor b in m_blocks) { result = result && b.Status == DoorStatus.Open; }
    return result;
  }

  public bool isClosed()
  {
    bool result = true;
    foreach (IMyDoor b in m_blocks) { result = result && b.Status == DoorStatus.Closed; }
    return result;
  }

  public void open () { foreach (IMyDoor b in m_blocks) { b.OpenDoor ();} }
  public void close() { foreach (IMyDoor b in m_blocks) { b.CloseDoor();} }


}
