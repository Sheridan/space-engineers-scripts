// #include classes/textsurface.cs
// #include helpers/datetime.cs
// #include classes/blocks/speaker.cs

public class CStateMachineState
{
  public CStateMachineState(string name, Func<object,bool> method, object data)
  {
    m_name    = name;
    m_method  = method;
    m_data    = data;
  }
  public bool callMethod() { return m_method(m_data); }
  public string name() { return m_name; }

  string m_name;
  Func<object,bool> m_method;
  object m_data;
}

public class CStateMachine
{
  public CStateMachine(CTextSurface lcd, CSpeaker speaker = null)
  {
    m_lcd = lcd;
    m_states = new List<CStateMachineState>();
    m_currentStateIndex = 0;
    m_waitCount = 0;
    m_selfDriven = false;
    m_speaker = speaker;
  }

  public void addState(string name, Func<object,bool> method, object data = null) { m_states.Add(new CStateMachineState(name, method, data)); m_currentStateIndex++; }
  private CStateMachineState state(int index) { return m_states[index]; }
  public CStateMachineState currentState() { return state(m_currentStateIndex); }

  private void switchToNextState()
  {
    playSound();
    m_waitCount = 0;
    m_currentStateIndex++;
    TimeSpan ts  = m_startDT     - System.DateTime.Now;
    TimeSpan tts = m_taskStartDT - System.DateTime.Now;
    m_lcd.echo($"[{currentTime()}] [{formatTimeSpan(ts)}] Задача {state(m_currentStateIndex-1).name()} завершена за время {formatTimeSpan(tts)}");
    if (active())
    {
      m_lcd.echo($"[{currentTime()}] [{formatTimeSpan(ts)}] Старт задачи '{state(m_currentStateIndex).name()}'");
    }
    else
    {
      m_lcd.echo($"[{currentTime()}] Алгоритм завершен. Длительность: {formatTimeSpan(ts)}");
      if (m_selfDriven) { self.Runtime.UpdateFrequency = UpdateFrequency.None; }
    }
    m_taskStartDT = System.DateTime.Now;
  }

  public void step()
  {
    TimeSpan ts = m_startDT - System.DateTime.Now;
    m_lcd.echo_at($"Текущее состояние: {currentState().name()} ({m_waitCount++}). Прошло времени: {formatTimeSpan(ts)}", 0);
    if(currentState().callMethod()) { switchToNextState(); }
  }

  public void start(bool selfDrivenMachine = false)
  {
    m_startDT     = System.DateTime.Now;
    m_taskStartDT = System.DateTime.Now;
    m_currentStateIndex = 0;
    m_selfDriven = selfDrivenMachine;
    m_lcd.echo($"[{currentTime()}] Алгоритм запущен");
    if(m_selfDriven) { self.Runtime.UpdateFrequency = UpdateFrequency.Update100; }
  }

  public void listStates()
  {
    foreach (CStateMachineState state in m_states)
    {
      m_lcd.echo(state.name());
    }
  }

  public bool active() { return m_currentStateIndex < m_states.Count; }

  private void playSound()
  {
    if(m_speaker != null)
    {
      m_speaker.play();
    }
  }

  private List<CStateMachineState> m_states;
  private int m_currentStateIndex;
  private CTextSurface m_lcd;
  private int m_waitCount;
  private bool m_selfDriven;
  private DateTime m_startDT;
  private DateTime m_taskStartDT;
  private CSpeaker m_speaker;
}
