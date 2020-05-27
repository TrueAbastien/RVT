using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Listener
{
    void update(string evt);
}

public class EventLauncher
{
    private List<Listener> m_list;
    private string m_name;

    public EventLauncher(string _name, params Listener[] lists)
    {
        m_name = _name;
        m_list = new List<Listener>(lists);
    }

    public void addListener(Listener list)
    {
        m_list.Add(list);
    }

    public void clearListener()
    {
        m_list.Clear();
    }
    
    public void notify()
    {
        foreach (Listener list in m_list)
            list.update(m_name);
    }
}
