using System.Collections.Generic;
using EmotivUnityPlugin;

public class ContactQualityHistory
{
    private const int HISTORY_MAX_COUNT = 10;
    private Dictionary<int, Queue<double>> history;

    public ContactQualityHistory()
    {
        history = new Dictionary<int, Queue<double>>() { };
    }

    public void Add(Channel_t channel, double value)
    {
        Queue<double> queue = Get(channel);

        while (queue.Count >= HISTORY_MAX_COUNT)
        {
            queue.Dequeue();
        }

        queue.Enqueue(value);
    }

    public Queue<double> Get(Channel_t channel)
    {
        foreach (KeyValuePair<int, Queue<double>> kvp in history)
        {
            if (kvp.Key == (int)channel)
            {
                return kvp.Value;
            }
        }

        Queue<double> queue = new Queue<double>();
        history.Add((int)channel, queue);

        return queue;
    }
}
