using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Drivers;
using AgateLib.Drivers.NullDrivers;
using AgateLib.Quality;

namespace AgateLib.Platform.Test
{
    public class FakeAgateFactory : IAgateFactory
    {
        private bool useRealFilesystem;

        public FakeAgateFactory() : this(new FakePlatformFactory())
        {

        }

        public FakeAgateFactory(FakePlatformFactory platformFactory)
        {
            DisplayFactory = new FakeDisplayFactory();
            AudioFactory = new FakeAudioFactory();
            InputFactory = new FakeInputFactory();
            PlatformFactory = platformFactory;
        }

        public FakeDisplayFactory DisplayFactory { get; private set; }
        public FakeAudioFactory AudioFactory { get; private set; }
        public FakeInputFactory InputFactory { get; private set; }
        public FakePlatformFactory PlatformFactory { get; private set; }

        public DisplayLib.FontSurface DefaultFont
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IDisplayFactory IAgateFactory.DisplayFactory
        {
            get { return DisplayFactory; }
        }

        IAudioFactory IAgateFactory.AudioFactory
        {
            get { return AudioFactory; }
        }

        IInputFactory IAgateFactory.InputFactory
        {
            get { return InputFactory; }
        }

        IPlatformFactory IAgateFactory.PlatformFactory
        {
            get { return PlatformFactory; }
        }
    }
}
