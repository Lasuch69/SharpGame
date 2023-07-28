public partial class Counter : Label
{
    private ulong _timestamp;

    public override void _Ready() => _timestamp = Time.GetTicksMsec();

    public override void _Process(double delta) => Text = GetTimeString(_timestamp);

    private string GetTimeString(ulong timestamp)
    {
        int timeSeconds = (int)((Time.GetTicksMsec() - timestamp) / 1000);
        int timeMinutes = timeSeconds / 60;

        string Format(int time) => String.Format("{0, 0:D2}", time);

        return String.Format("{0}:{1}", Format(timeMinutes), Format(timeSeconds % 60));
    }
}
