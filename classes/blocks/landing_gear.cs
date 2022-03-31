// #include classes/blocks/base/functional.cs

public class CLandingGear : CFunctional<IMyLandingGear>
{
  public CLandingGear(CBlocksBase<IMyLandingGear> blocks) : base(blocks) { }

  public bool lockGear(bool enabled = true)
  {
    foreach (IMyLandingGear b in m_blocks)
    {
      if (enabled) { b.Lock(); } else { b.Unlock(); }
    }
    return checkLocked(enabled);
  }
  public bool unlockGear() { return lockGear(false); }

  public bool locked() { return checkLocked(); }

  private bool checkLocked(bool target = true)
  {
    bool result = true;
    foreach (IMyLandingGear b in m_blocks)
    {
      result = result && b.IsLocked;
    }
    return result == target;
  }
}
