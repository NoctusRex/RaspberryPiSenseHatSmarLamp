using Android.Bluetooth;
using System;

namespace SmarLamp.Bluetooth
{
    public class CharacteristcEventArgs : EventArgs
    {
        public CharacteristcEventArgs(Java.Util.UUID uuid, byte[] data, ProfileState newState)
        {
            CharacteristcsUuid = uuid;
            Data = data;
            NewState = NewState;
        }

        public Java.Util.UUID CharacteristcsUuid { get; set; }
        public byte[] Data { get; set; }
        public ProfileState NewState { get; set; }
    }
}
