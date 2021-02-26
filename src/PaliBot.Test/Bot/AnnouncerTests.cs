using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaliBot.ApiClient;
using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Model.Play;
using PaliBot.Sensors.Event;
using PaliBot.Sensors.Play;
using PaliBot.Sensors.State;
using PaliBot.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Utf8Json;

namespace PaliBot.Test.Bot
{
    [TestClass]
    public class AnnouncerTests
    {
        private const string SAMPLE_SESSION = "{\"disc\":{\"position\":[1.0,4.5360003,27.500002],\"forward\":[0.001,-0.001,1.0],\"left\":[1.0,0.001,-0.001],\"up\":[-0.001,1.0,0.001],\"velocity\":[1.0,2.0,3.0],\"bounce_count\":1},\"sessionid\":\"B113374B-A8C5-40B5-9D3D-AFB2B26ECEFE\",\"orange_team_restart_request\":0,\"sessionip\":\"127.0.0.1\",\"game_status\":\"round_start\",\"game_clock_display\":\"02:44.02\",\"game_clock\":164.02505,\"match_type\":\"Echo_Arena\",\"map_name\":\"mpl_arena_a\",\"private_match\":false,\"orange_points\":3,\"player\":{\"vr_left\":[-0.99600005,0.0,-0.088000007],\"vr_position\":[0.26900002,4.671,52.986004],\"vr_forward\":[0.088000007,-0.001,-0.99600005],\"vr_up\":[0.0,1.0,-0.001]},\"pause\":{\"paused_state\":\"paused\",\"unpaused_team\":\"BLUE TEAM\",\"paused_requested_team\":\"ORANGE TEAM\",\"unpaused_timer\":7.0,\"paused_timer\":76.0},\"possession\":[0,2],\"tournament_match\":false,\"blue_team_restart_request\":0,\"client_name\":\"tickleBritches\",\"blue_points\":14,\"last_score\":{\"disc_speed\":18.2,\"team\":\"orange\",\"goal_type\":\"INSIDE SHOT\",\"point_amount\":2,\"distance_thrown\":7.4,\"person_scored\":\"MrMrsTejada\",\"assist_scored\":\"NuclearApple\"},\"teams\":[{\"players\":[{\"rhand\":{\"pos\":[0.57600003,4.4050002,-50.778004],\"forward\":[-0.82000005,-0.45500001,0.34800002],\"left\":[0.333,0.116,0.93600005],\"up\":[-0.46600002,0.88300002,0.056000002]},\"playerid\":2,\"name\":\"Carvidsanders\",\"userid\":352342346675,\"stats\":{\"possession_time\":9.850441,\"points\":2,\"saves\":1,\"goals\":1,\"stuns\":1,\"passes\":1,\"catches\":1,\"steals\":1,\"blocks\":1,\"interceptions\":1,\"assists\":1,\"shots_taken\":1},\"number\":69,\"level\":28,\"stunned\":true,\"ping\":44,\"invulnerable\":true,\"head\":{\"position\":[0.73700005,4.8530002,-50.740002],\"forward\":[-0.54500002,-0.23900001,-0.80300003],\"left\":[-0.80400002,-0.12400001,0.58200002],\"up\":[-0.23900001,0.96300006,-0.125]},\"possession\":true,\"body\":{\"position\":[0.73700005,4.8530002,-50.740002],\"forward\":[-0.78300005,0.0,-0.62300003],\"left\":[-0.62300003,0.0020000001,0.78300005],\"up\":[0.001,1.0,-0.001]},\"lhand\":{\"pos\":[0.85200006,4.5930004,-50.083004],\"forward\":[-0.13000001,0.39200002,0.91100007],\"left\":[0.97500002,0.21900001,0.045000002],\"up\":[-0.18200001,0.89300007,-0.41100001]},\"blocking\":true,\"velocity\":[0.045000002,-0.045000002,-0.091000006]},{\"rhand\":{\"pos\":[-0.090000004,4.1950002,-53.572002],\"forward\":[0.20200001,-0.0020000001,0.97900003],\"left\":[0.70900005,-0.69000006,-0.148],\"up\":[0.67600006,0.72400004,-0.13800001]},\"playerid\":3,\"name\":\"0riginal\",\"userid\":1428512207236505,\"stats\":{\"possession_time\":28.825672,\"points\":8,\"saves\":0,\"goals\":0,\"stuns\":4,\"passes\":0,\"catches\":0,\"steals\":0,\"blocks\":0,\"interceptions\":0,\"assists\":1,\"shots_taken\":4},\"number\":11,\"level\":50,\"stunned\":false,\"ping\":35,\"invulnerable\":false,\"head\":{\"position\":[-0.049000002,4.5600004,-54.097004],\"forward\":[0.091000006,-0.078000002,0.99300003],\"left\":[0.99100006,0.104,-0.083000004],\"up\":[-0.097000003,0.99200004,0.086000003]},\"possession\":false,\"body\":{\"position\":[-0.049000002,4.5600004,-54.097004],\"forward\":[0.125,0.040000003,0.99100006],\"left\":[0.99100006,-0.047000002,-0.123],\"up\":[0.042000003,0.99800003,-0.045000002]},\"lhand\":{\"pos\":[0.127,3.7360001,-54.000004],\"forward\":[-0.116,-0.66300005,0.74000001],\"left\":[0.89200002,0.25800002,0.37100002],\"up\":[-0.43700001,0.70300001,0.56100005]},\"blocking\":false,\"velocity\":[-0.090000004,-0.36000001,0.0]},{\"rhand\":{\"pos\":[-0.30600002,3.5510001,-53.283001],\"forward\":[-0.45400003,-0.63600004,0.62400001],\"left\":[0.30500001,-0.76900005,-0.56200004],\"up\":[0.83700001,-0.065000005,0.54300004]},\"playerid\":4,\"name\":\"CobinVR\",\"userid\":35280228564671112,\"stats\":{\"possession_time\":15.405767,\"points\":6,\"saves\":0,\"goals\":0,\"stuns\":1,\"passes\":0,\"catches\":0,\"steals\":1,\"blocks\":0,\"interceptions\":0,\"assists\":0,\"shots_taken\":3},\"number\":0,\"level\":50,\"stunned\":false,\"ping\":32,\"invulnerable\":false,\"head\":{\"position\":[-0.093000002,4.4120002,-53.391003],\"forward\":[0.076000005,-0.093000002,0.99300003],\"left\":[0.99500006,0.066,-0.07],\"up\":[-0.059000004,0.99400002,0.097000003]},\"possession\":true,\"body\":{\"position\":[-0.093000002,4.4120002,-53.391003],\"forward\":[0.063000001,0.001,0.99800003],\"left\":[0.99800003,-0.0020000001,-0.063000001],\"up\":[0.0020000001,1.0,-0.001]},\"lhand\":{\"pos\":[0.074000001,3.6700001,-53.254002],\"forward\":[0.16500001,-0.68300003,0.71200001],\"left\":[0.90200007,0.39700001,0.17200001],\"up\":[-0.40000001,0.61400002,0.68100005]},\"blocking\":false,\"velocity\":[0.0,-0.045000002,-0.047000002]},{\"rhand\":{\"pos\":[0.50300002,4.1200004,-51.135002],\"forward\":[0.050000001,-0.39900002,0.91600007],\"left\":[0.98300004,0.18200001,0.026000001],\"up\":[-0.177,0.89900005,0.40100002]},\"playerid\":6,\"name\":\"Acadie\",\"userid\":3097913126765858,\"stats\":{\"possession_time\":0.0,\"points\":0,\"saves\":0,\"goals\":0,\"stuns\":8,\"passes\":0,\"catches\":0,\"steals\":0,\"blocks\":0,\"interceptions\":0,\"assists\":0,\"shots_taken\":0},\"number\":57,\"level\":50,\"stunned\":false,\"ping\":33,\"invulnerable\":false,\"head\":{\"position\":[0.65900004,4.7800002,-51.256004],\"forward\":[-0.043000001,-0.0070000002,0.99900007],\"left\":[0.99700004,0.062000003,0.043000001],\"up\":[-0.062000003,0.99800003,0.0050000004]},\"possession\":false,\"body\":{\"position\":[0.65900004,4.7800002,-51.256004],\"forward\":[0.039000001,0.001,0.99900007],\"left\":[0.99900007,-0.001,-0.039000001],\"up\":[0.001,1.0,-0.001]},\"lhand\":{\"pos\":[0.88000005,4.4900002,-50.889004],\"forward\":[0.12100001,0.55900002,0.82000005],\"left\":[0.88800007,0.30800003,-0.34100002],\"up\":[-0.44300002,0.77000004,-0.45900002]},\"blocking\":false,\"velocity\":[0.40400001,0.22400001,0.22500001]}],\"team\":\"BLUE TEAM\",\"possession\":true,\"stats\":{\"points\":14,\"possession_time\":54.081879,\"interceptions\":1,\"blocks\":2,\"steals\":3,\"catches\":4,\"passes\":5,\"saves\":6,\"goals\":7,\"stuns\":15,\"assists\":1,\"shots_taken\":7}},{\"players\":[{\"rhand\":{\"pos\":[0.33500001,3.6510003,51.018002],\"forward\":[-0.91600007,-0.048,-0.39800003],\"left\":[-0.1,-0.93400002,0.34200001],\"up\":[-0.38800001,0.354,0.85100001]},\"playerid\":0,\"name\":\"Elihathebot678\",\"userid\":471824779144825,\"stats\":{\"possession_time\":0.0,\"points\":0,\"saves\":0,\"goals\":0,\"stuns\":4,\"passes\":0,\"catches\":0,\"steals\":0,\"blocks\":0,\"interceptions\":0,\"assists\":0,\"shots_taken\":0},\"number\":0,\"level\":50,\"stunned\":false,\"ping\":103,\"invulnerable\":false,\"head\":{\"position\":[0.50400001,4.2530003,51.196003],\"forward\":[-0.80100006,-0.25300002,-0.54300004],\"left\":[-0.54900002,-0.050000001,0.83400005],\"up\":[-0.23800001,0.96600002,-0.099000007]},\"possession\":false,\"body\":{\"position\":[0.50400001,4.2530003,51.196003],\"forward\":[-0.58500004,-0.0020000001,-0.81100005],\"left\":[-0.81100005,0.0,0.58500004],\"up\":[-0.001,1.0,-0.001]},\"lhand\":{\"pos\":[0.67000002,3.5670002,51.608002],\"forward\":[0.38000003,-0.36500001,0.85000002],\"left\":[-0.10600001,0.89600003,0.43200001],\"up\":[-0.91900003,-0.25400001,0.30100003]},\"blocking\":false,\"velocity\":[1.4360001,0.0,2.3800001]},{\"rhand\":{\"pos\":[0.20200001,4.0880003,50.514004],\"forward\":[-0.21400002,-0.186,-0.95900005],\"left\":[-0.80100006,-0.528,0.28100002],\"up\":[-0.55900002,0.82900006,-0.036000002]},\"playerid\":1,\"name\":\"MrMrsTejada\",\"userid\":285278394681732,\"stats\":{\"possession_time\":33.568836,\"points\":3,\"saves\":1,\"goals\":0,\"stuns\":1,\"passes\":0,\"catches\":0,\"steals\":0,\"blocks\":0,\"interceptions\":0,\"assists\":0,\"shots_taken\":1},\"number\":12,\"level\":50,\"stunned\":false,\"ping\":36,\"invulnerable\":false,\"head\":{\"position\":[0.095000006,4.5690002,50.816002],\"forward\":[-0.10200001,-0.30400002,-0.94700003],\"left\":[-0.98800004,-0.082000002,0.133],\"up\":[-0.11800001,0.94900006,-0.29200003]},\"possession\":false,\"body\":{\"position\":[0.095000006,4.5690002,50.816002],\"forward\":[-0.14500001,0.071000002,-0.98700005],\"left\":[-0.98300004,-0.12200001,0.13600001],\"up\":[-0.11100001,0.99000007,0.088000007]},\"lhand\":{\"pos\":[-0.0070000002,3.6780002,50.712002],\"forward\":[-0.23,-0.75300002,-0.61700004],\"left\":[-0.83000004,0.48200002,-0.27900001],\"up\":[0.50800002,0.44800001,-0.73600006]},\"blocking\":false,\"velocity\":[-0.090000004,0.090000004,-0.134]},{\"rhand\":{\"pos\":[0.28800002,2.7470002,55.748001],\"forward\":[0.48000002,0.47800002,-0.73500001],\"left\":[-0.74100006,-0.22800002,-0.63200003],\"up\":[-0.47000003,0.84800005,0.245]},\"playerid\":5,\"name\":\"NuclearApple\",\"userid\":2947498732050709,\"stats\":{\"possession_time\":15.329506,\"points\":0,\"saves\":0,\"goals\":0,\"stuns\":2,\"passes\":0,\"catches\":0,\"steals\":1,\"blocks\":0,\"interceptions\":0,\"assists\":0,\"shots_taken\":0},\"number\":17,\"level\":50,\"stunned\":false,\"ping\":223,\"invulnerable\":false,\"head\":{\"position\":[0.014,2.9530001,56.193001],\"forward\":[0.11700001,-0.104,-0.98800004],\"left\":[-0.99300003,-0.024,-0.115],\"up\":[-0.012,0.99400002,-0.10600001]},\"possession\":false,\"body\":{\"position\":[0.014,2.9530001,56.193001],\"forward\":[0.148,-0.001,-0.98900002],\"left\":[-0.98900002,-0.0020000001,-0.148],\"up\":[-0.0020000001,1.0,-0.001]},\"lhand\":{\"pos\":[-0.13600001,2.4030001,56.084003],\"forward\":[-0.38900003,-0.014,-0.92100006],\"left\":[-0.85000002,0.39100003,0.35300002],\"up\":[0.35500002,0.92000002,-0.164]},\"blocking\":false,\"velocity\":[0.266,0.46000001,-1.113]},{\"rhand\":{\"pos\":[0.26800001,3.6200001,50.215004],\"forward\":[0.31900001,-0.73000002,-0.60400003],\"left\":[-0.69900006,-0.61200005,0.37],\"up\":[-0.64000005,0.30400002,-0.70500004]},\"playerid\":7,\"name\":\"Lolo_\",\"userid\":2664175320342364,\"stats\":{\"possession_time\":16.688866,\"points\":0,\"saves\":1,\"goals\":0,\"stuns\":4,\"passes\":0,\"catches\":0,\"steals\":0,\"blocks\":0,\"interceptions\":0,\"assists\":0,\"shots_taken\":0},\"number\":19,\"level\":50,\"stunned\":false,\"ping\":46,\"invulnerable\":false,\"head\":{\"position\":[0.11700001,4.4100003,50.351002],\"forward\":[-0.091000006,-0.162,-0.98300004],\"left\":[-0.98600006,0.15300001,0.066],\"up\":[0.14,0.97500002,-0.17400001]},\"possession\":false,\"body\":{\"position\":[0.11700001,4.4100003,50.351002],\"forward\":[-0.084000006,-0.001,-0.99700004],\"left\":[-0.99600005,-0.0020000001,0.084000006],\"up\":[-0.0020000001,1.0,-0.001]},\"lhand\":{\"pos\":[-0.014,4.3330002,50.474003],\"forward\":[-0.18400002,0.88400006,0.42900002],\"left\":[-0.98300004,-0.15400001,-0.10300001],\"up\":[-0.025,-0.44100001,0.89700001]},\"blocking\":false,\"velocity\":[0.072000004,0.036000002,0.0]}],\"team\":\"ORANGE TEAM\",\"possession\":false,\"stats\":{\"points\":3,\"possession_time\":65.587204,\"interceptions\":0,\"blocks\":0,\"steals\":1,\"catches\":0,\"passes\":0,\"saves\":2,\"goals\":0,\"stuns\":11,\"assists\":0,\"shots_taken\":1}}]}";

        [TestMethod]
        public void CreatesSensors()
        {
            var announcer = new Announcer();

            //

            Assert.IsNotNull(announcer._stateSensors.OfType<PossessionStateSensor>().Single());

            Assert.IsNotNull(announcer._eventSensors.OfType<PossessionChangeEventSensor>().Single());
        }

        [TestMethod]
        public void InitializesEventSensors()
        {
            var mockEventSensor = new Mock<IEventSensor>();

            var mockEventSensorFactory = new Mock<IEventSensorFactory>();
            mockEventSensorFactory.Setup(x => x.Create<IsEventSensor>()).Returns(mockEventSensor.Object);

            //
            var announcer = new Announcer(new SessionConverter(), new StateSensorFactory(), mockEventSensorFactory.Object, new PlaySensorFactory());
            //

            mockEventSensor.Verify(x => x.Initialize(announcer._stateSensors), Times.Exactly(announcer._eventSensors.Length));
        }

        [TestMethod]
        public void InitializesPlaySensors()
        {
            var mockPlaySensor = new Mock<IPlaySensor>();

            var mockPlaySensorFactory = new Mock<IPlaySensorFactory>();
            mockPlaySensorFactory.Setup(x => x.Create<IsPlaySensor>()).Returns(mockPlaySensor.Object);

            //
            var announcer = new Announcer(new SessionConverter(), new StateSensorFactory(), new EventSensorFactory(), mockPlaySensorFactory.Object);
            //

            mockPlaySensor.Verify(x => x.Initialize(announcer._stateSensors), Times.Exactly(announcer._playSensors.Length));
        }

        [TestMethod]
        public void Update_UpdatesSensors()
        {
            var session = JsonSerializer.Deserialize<Session>(SAMPLE_SESSION);
            var frame = new Frame();

            var mockSessionConverter = new Mock<ISessionConverter>();
            
            var mockStateSensorFactory = new Mock<IStateSensorFactory>();
            var mockEventSensorFactory = new Mock<IEventSensorFactory>();
            var mockPlaySensorFactory = new Mock<IPlaySensorFactory>();

            var mockStateSensor = new Mock<IStateSensor>();
            var mockEventSensor = new Mock<IEventSensor>();
            var mockPlaySensor = new Mock<IPlaySensor>();

            mockSessionConverter.Setup(x => x.ToFrame(session)).Returns(frame);

            mockStateSensorFactory.Setup(x => x.Create<IsStateSensor>()).Returns(mockStateSensor.Object);
            mockEventSensorFactory.Setup(x => x.Create<IsEventSensor>()).Returns(mockEventSensor.Object);
            mockPlaySensorFactory.Setup(x => x.Create<IsPlaySensor>()).Returns(mockPlaySensor.Object);

            mockEventSensor.Setup(x => x.Update(frame)).Returns(new TestGameEvent(frame));
            mockPlaySensor.Setup(x => x.Update(frame, It.IsAny<IEnumerable<GameEvent>>())).Returns(new TestGamePlay(0));

            var announcementCount = 0;

            var announcer = new Announcer(mockSessionConverter.Object, mockStateSensorFactory.Object, mockEventSensorFactory.Object, mockPlaySensorFactory.Object);
            announcer.Announce += (sender, announcement) =>
            {
                announcementCount++;
            };

            //
            announcer.Update(session);
            //

            mockStateSensor.Verify(x => x.Update(frame), Times.Exactly(announcer._stateSensors.Length));
            mockEventSensor.Verify(x => x.Update(frame), Times.Exactly(announcer._eventSensors.Length));
            mockPlaySensor.Verify(x => x.Update(frame, It.IsAny<IEnumerable<GameEvent>>()), Times.Exactly(announcer._playSensors.Length));

            Assert.AreEqual(announcer._playSensors.Length, announcementCount);
        }

        [TypeMatcher]
        private class IsStateSensor : IStateSensor, ITypeMatcher
        {
            public bool Matches(Type typeArgument)
            {
                return typeof(IStateSensor).IsAssignableFrom(typeArgument);
            }

            public void Update(IFrame frame)
            {
                throw new NotImplementedException();
            }
        }

        [TypeMatcher]
        private class IsEventSensor : IEventSensor, ITypeMatcher
        {
            public bool Matches(Type typeArgument)
            {
                return typeof(IEventSensor).IsAssignableFrom(typeArgument);
            }

            public void Initialize(IEnumerable<IStateSensor> stateSensors)
            {
                throw new NotImplementedException();
            }

            public GameEvent Update(IFrame frame)
            {
                throw new NotImplementedException();
            }
        }

        [TypeMatcher]
        private class IsPlaySensor : IPlaySensor, ITypeMatcher
        {
            public bool Matches(Type typeArgument)
            {
                return typeof(IPlaySensor).IsAssignableFrom(typeArgument);
            }

            public void Initialize(IEnumerable<IStateSensor> stateSensors)
            {
                throw new NotImplementedException();
            }

            public GamePlay Update(IFrame frame, IEnumerable<GameEvent> gameEvents)
            {
                throw new NotImplementedException();
            }
        }

        private class TestGameEvent : GameEvent
        {
            public TestGameEvent(IFrame frame) : base(frame)
            {
            }
        }

        private class TestGamePlay : GamePlay
        {
            public TestGamePlay(int priority) : base(priority)
            {
            }

            public override string Describe()
            {
                return String.Empty;
            }
        }
    }
}
