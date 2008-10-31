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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Input.Old
{
    /// <summary>
    /// Class which encapsulates functionality associated with a set of commands.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InputState<T> : IDisposable 
        // \cond doxygenignore
        where T : struct
        // \endcond
    {
        Dictionary<T, InputCommand<T>> mCommands = new Dictionary<T, InputCommand<T>>();
        bool mAutoUpdate;

        /// <summary>
        /// Constructs an InputState object.
        /// </summary>
        public InputState()
        {
            AutoUpdate = true;
        }

        /// <summary>
        /// Disposes of this object.
        /// </summary>
        public void Dispose()
        {
            AutoUpdate = false;
        }
        /// <summary>
        /// Creates and adds a new input command with the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        public void Add(T id)
        {
            InputCommand<T> val = new InputCommand<T>();

            val.Identifier = id;

            mCommands.Add(id, val);
        }

        /// <summary>
        /// Gets or sets the specified input command.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InputCommand<T> this[T id]
        {
            get
            {
                if (mCommands.ContainsKey(id) == false)
                    Add(id);

                return mCommands[id];
            }
            set { mCommands[id] = value; }
        }

        /// <summary>
        /// Updates each of the input commands' state.
        /// </summary>
        public void Update()
        {
            foreach (InputCommand<T> ic in mCommands.Values)
            {
                ic.Update();
            }
        }

        /// <summary>
        /// Returns how many commands are currently set on.. ie. the user is 
        /// pressing buttons for them.
        /// </summary>
        public int PushedCommandCount
        {
            get
            {
                int retval = 0;

                foreach (InputCommand<T> ic in mCommands.Values)
                {
                    if (ic.Value)
                        retval++;
                }

                return retval;
            }
        }

        /// <summary>
        /// Gets or sets whether or not this InputState should automatically update.
        /// If you are done with it, you should set this to false, or call Dispose().
        /// </summary>
        public bool AutoUpdate
        {
            get { return mAutoUpdate; }
            set
            {
                if (value != mAutoUpdate )
                {
                    if (value)
                        Input.JoystickPollEvent += new Input.JoystickPollDelegate(Update);
                    else
                        Input.JoystickPollEvent -= Update;

                    mAutoUpdate = value;
                }
            }
        }
    }
}