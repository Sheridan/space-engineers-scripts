// #include classes/textsurface.cs
// #include helpers/datetime.cs

public class CStateMachineState
{
  public CStateMachineState(string name, Func<object,bool> method, object data)
  {
    m_name = name;
    m_method = method;
    m_data = data;
  }
  public bool callMethod() { return m_method(m_data); }
  public string name() { return m_name; }

  string m_name;
  Func<object,bool> m_method;
  object m_data;
}

public class CStateMachine
{
  public CStateMachine(CTextSurface lcd)
  {
    m_lcd = lcd;
    m_states = new List<CStateMachineState>();
    m_currentStateIndex = 0;
    waitCount = 0;
    selfDriven = false;
  }

  public void addState(string name, Func<object,bool> method, object data = null) { m_states.Add(new CStateMachineState(name, method, data)); m_currentStateIndex++; }
  private CStateMachineState state(int index) { return m_states[index]; }
  public CStateMachineState currentState() { return state(m_currentStateIndex); }

  private void switchToNextState()
  {
    waitCount = 0;
    m_currentStateIndex++;
    if (active())
    {
      m_lcd.echo($"[{currentTime()}] Переключение состояния с {state(m_currentStateIndex-1).name()} на {state(m_currentStateIndex).name()}");
    }
    else
    {
      m_lcd.echo($"[{currentTime()}] Алгоритм завершен");
      if (selfDriven) { self.Runtime.UpdateFrequency = UpdateFrequency.None; }
    }
  }

  public void step()
  {
    m_lcd.echo_at($"Текущее состояние: {currentState().name()} ({waitCount++})", 0);
    if(currentState().callMethod()) { switchToNextState(); }
  }

  public void start(bool selfDrivenMachine = false)
  {
    m_currentStateIndex = 0;
    selfDriven = selfDrivenMachine;
    m_lcd.echo($"[{currentTime()}] Алгоритм запущен");
    if(selfDriven) { self.Runtime.UpdateFrequency = UpdateFrequency.Update100; }
  }

  public void listStates()
  {
    foreach (CStateMachineState state in m_states)
    {
      m_lcd.echo(state.name());
    }
  }

  public bool active() { return m_currentStateIndex < m_states.Count; }

  private List<CStateMachineState> m_states;
  private int m_currentStateIndex;
  private CTextSurface m_lcd;
  private int waitCount;
  private bool selfDriven;
}
