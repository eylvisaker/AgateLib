using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Drivers
{
    public enum SetSystemsDialogResult
    {
        OK,
        Cancel,
    }
    public interface IUserSetSystems
    {
        void AddDisplayType(DriverInfo<DisplayTypeID> info);
        void AddAudioType(DriverInfo<AudioTypeID> info);
        void AddInputType(DriverInfo<InputTypeID> info);

        void SetDefaultDisplay(DriverInfo<DisplayTypeID> highestDisplay);
        void SetDefaultAudio(DriverInfo<AudioTypeID> highestAudio);
        void SetDefaultInput(DriverInfo<InputTypeID> highestInput);

        SetSystemsDialogResult RunDialog();

        DisplayTypeID DisplayType { get; }
        AudioTypeID AudioType { get; }
        InputTypeID InputType { get; }

        void SetChoices(bool chooseDisplay, bool chooseAudio, bool chooseInput);
        

    }
}
