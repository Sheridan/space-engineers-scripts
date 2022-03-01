// #include classes/blocks/functional.cs

public class CLandingGear : CFunctional<IMyLandingGear>
{
  public CLandingGear(CBlocksBase<IMyLandingGear> blocks) : base(blocks) { }

  public bool lockGear(bool enabled = true)
  {
    bool result = true;
    foreach (IMyLandingGear lg in m_blocks.blocks())
    {
      if (enabled) { lg.Lock(); } else { lg.Unlock(); }
      result = result && lg.IsLocked;
    }
    return result;
  }
  public bool unlockGear() { return lockGear(false); }

}
