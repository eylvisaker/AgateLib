//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.ApplicationModels
{
	/// <summary>
	/// The passive model does very little - it simply initializes AgateLib and cleans up when your
	/// program exits. The passive model is suitable for applications which provide their own message
	/// pump and render loop logic. 
	/// </summary>
	[Obsolete("Use new AgateSetup object instead.", true)]
	public class PassiveModel : EntryPointAppModelBase
	{
		#region --- Static Members ---

		static PassiveModel()
		{
			DefaultParameters = new PassiveModelParameters();
		}

		
		public static PassiveModelParameters DefaultParameters { get; set; }

		#endregion

		public PassiveModel() : this(DefaultParameters)
		{ }

		public PassiveModel(PassiveModelParameters parameters)
			: base(parameters)
		{
		}
		public PassiveModel(string[] args) : this(DefaultParameters)
		{
			Parameters.Arguments = args;

			ProcessArguments();
		}

		protected override void InitializeImpl()
		{
			new WinFormsInitializer().Initialize(Parameters);
		}

		public new PassiveModelParameters Parameters
		{
			get { return (PassiveModelParameters)base.Parameters; }
		}

		protected override int BeginModel(Func<int> entryPoint)
		{
			return entryPoint();
		}


		public override void KeepAlive()
		{
			System.Windows.Forms.Application.DoEvents();

			base.KeepAlive();
		}
	}
}
