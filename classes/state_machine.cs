// #include classes/textsurface.cs

public class CStateMachineState
{
  public CStateMachineState(string name, Func<bool> method)
  {
    m_name = name;
    m_method = method;
  }
  public bool callMethod() { return m_method(); }
  public string name() { return m_name; }

  string m_name;
  Func<bool> m_method;
}

public class CStateMachine
{
  public CStateMachine(CTextSurface lcd, int defaultState = 0)
  {
    m_lcd = lcd;
    m_states = new List<CStateMachineState>();
    m_currentStateIndex = defaultState-1;
  }

  public void addState(string name, Func<bool> method) { m_states.Add(new CStateMachineState(name, method)); }
  private CStateMachineState state(int index) { return m_states[index]; }
  public CStateMachineState currentState() { return state(m_currentStateIndex); }

  private void switchToNextState()
  {
    if(m_currentStateIndex < m_states.Count-1)
    {
      m_lcd.echo($"Переключение состояния с {state(m_currentStateIndex).name()} на {state(m_currentStateIndex+1).name()}");
      m_currentStateIndex++;
    }
  }

  public void step()
  {
    m_lcd.echo_at($"Текущее состояние: {currentState().name()}", 0);
    if(currentState().callMethod()) { switchToNextState(); }
  }
  public void start() { m_currentStateIndex = 0; }

  public void listStates()
  {
    foreach (CStateMachineState state in m_states)
    {
      m_lcd.echo(state.name());
    }
  }

  private List<CStateMachineState> m_states;
  private int m_currentStateIndex;
  private CTextSurface m_lcd;
}
