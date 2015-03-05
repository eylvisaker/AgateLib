using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.DisplayLib;
using AgateLib.Drivers;

namespace AgateLib.UnitTests.Fakes
{
    public class FakeFactory : IAgateFactory
    {
        FakeDisplayFactory displayFactory = new FakeDisplayFactory();
        FakeAudioFactory audioFactory = new FakeAudioFactory();
        FakeInputFactory inputFactory = new FakeInputFactory();
        FakePlatformFactory platformFactory = new FakePlatformFactory();

        public FakeDisplayFactory DisplayFactory { get { return displayFactory; } }
        public FakeAudioFactory AudioFactory { get { return audioFactory; } }
        public FakeInputFactory InputFactory { get { return inputFactory; } }
        public FakePlatformFactory PlatformFactory { get { return platformFactory; } }

        IDisplayFactory IAgateFactory.DisplayFactory { get { return displayFactory; } }
        IAudioFactory IAgateFactory.AudioFactory { get { return audioFactory; } }
        IInputFactory IAgateFactory.InputFactory { get { return inputFactory; } }
        IPlatformFactory IAgateFactory.PlatformFactory { get { return platformFactory; } }


    }
}
