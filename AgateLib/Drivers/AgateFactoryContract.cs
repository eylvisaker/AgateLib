using System;
using System.Diagnostics.Contracts;

namespace AgateLib.Drivers
{
	[ContractClassFor(typeof(IAgateFactory))]
	internal abstract class AgateFactoryContract : IAgateFactory
	{
		public IAudioFactory AudioFactory
		{
			get
			{
				Contract.Ensures(Contract.Result<IAudioFactory>() != null);

				throw new NotImplementedException();
			}
		}

		public IDisplayFactory DisplayFactory
		{
			get
			{
				Contract.Ensures(Contract.Result<IDisplayFactory>() != null);

				throw new NotImplementedException();
			}
		}

		public IInputFactory InputFactory
		{
			get
			{
				Contract.Ensures(Contract.Result<IInputFactory>() != null);

				throw new NotImplementedException();
			}
		}

		public IPlatformFactory PlatformFactory
		{
			get
			{
				Contract.Ensures(Contract.Result<IPlatformFactory>() != null);

				throw new NotImplementedException();
			}
		}
	}
}