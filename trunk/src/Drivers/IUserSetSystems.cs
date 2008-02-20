using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Drivers
{
    /// <summary>
    /// Enum for the results of a call to IUserSetSystem.RunDialog
    /// </summary>
    public enum SetSystemsDialogResult
    {
        /// <summary>
        /// Value for when the user pressed OK
        /// </summary>
        OK,
        /// <summary>
        /// Value for when the user pressed CANCEL
        /// </summary>
        Cancel,
    }
    /// <summary>
    /// Interface for asking the user which Agate drivers to invoke.
    /// </summary>
    public interface IUserSetSystems
    {
        /// <summary>
        /// Adds a Display driver to the possible options.
        /// </summary>
        /// <param name="info"></param>
        void AddDisplayType(DriverInfo<DisplayTypeID> info);
        /// <summary>
        /// Adds a Audio driver to the possible options.
        /// </summary>
        /// <param name="info"></param>
        void AddAudioType(DriverInfo<AudioTypeID> info);
        /// <summary>
        /// Adds a Input driver to the possible options.
        /// </summary>
        /// <param name="info"></param>
        void AddInputType(DriverInfo<InputTypeID> info);

        /// <summary>
        /// Sets the default Display driver.
        /// </summary>
        /// <param name="highestDisplay"></param>
        void SetDefaultDisplay(DriverInfo<DisplayTypeID> highestDisplay);
        /// <summary>
        /// Sets the default Audio driver.
        /// </summary>
        /// <param name="highestAudio"></param>
        void SetDefaultAudio(DriverInfo<AudioTypeID> highestAudio);
        /// <summary>
        /// Sets the default Input driver.
        /// </summary>
        /// <param name="highestInput"></param>
        void SetDefaultInput(DriverInfo<InputTypeID> highestInput);

        /// <summary>
        /// Shows the dialog asking the user what drivers to use.
        /// </summary>
        /// <returns></returns>
        SetSystemsDialogResult RunDialog();

        /// <summary>
        /// gets the selected Display driver
        /// </summary>
        DisplayTypeID DisplayType { get; }
        /// <summary>
        /// Gets the selected Audio driver
        /// </summary>
        AudioTypeID AudioType { get; }
        /// <summary>
        /// Gets the selected Input driver
        /// </summary>
        InputTypeID InputType { get; }

        /// <summary>
        /// Sets which choices the user is presented with.
        /// </summary>
        /// <param name="chooseDisplay"></param>
        /// <param name="chooseAudio"></param>
        /// <param name="chooseInput"></param>
        void SetChoices(bool chooseDisplay, bool chooseAudio, bool chooseInput);
        

    }
}
