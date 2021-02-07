namespace PaliBot.Model
{
    public interface IClientInfo
    {
        string Name { get; }
        IPose Pose { get; }
    }

    public class ClientInfo : IClientInfo
    {
        public string Name { get; set; }
        public Pose Pose { get; set; } = new Pose();

        IPose IClientInfo.Pose => Pose;
    }
}
