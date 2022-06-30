using System.Collections.Generic;

public class GroupedSignals
{
    private Dictionary<(int, int), List<double>> values;

    public GroupedSignals()
    {
        values = new Dictionary<(int, int), List<double>>() { };
    }

    public void Add(CerebrumArea.CerebrumArea_t cerebrumArea, Band.Band_t band, double value)
    {
        List<double> list = Get(cerebrumArea, band);
        list.Add(value);
    }

    public List<double> Get(CerebrumArea.CerebrumArea_t cerebrumArea, Band.Band_t band)
    {
        foreach (KeyValuePair<(int, int), List<double>> kvp in values)
        {
            if (kvp.Key.Item1 == (int)cerebrumArea && kvp.Key.Item2 == (int)band)
            {
                return kvp.Value;
            }
        }

        List<double> list = new List<double>();
        values.Add(((int)cerebrumArea, (int)band), list);

        return list;
    }

    
    public void DeleteAll()
    {
        foreach (KeyValuePair<(int, int), List<double>> kvp in values)
        {
            List<double> list = kvp.Value;
            list.Clear();
        }
    }
}
