using Android.Bluetooth;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmarLamp.Bluetooth
{
    public class BluetoothClient
    {
        public IBluetoothLE Bluetooth { get; set; }
        private IAdapter Adapter { get; set; }
        public BluetoothManager Manager { get; set; }
        private IDevice ConnectedDevice { get; set; }
        private BluetoothGatt Gatt { get; set; }
        private GattCallback GattCallback { get; set; } = new GattCallback();

        public event EventHandler<CharacteristcEventArgs> ConnectStateChanged = delegate { };
        public event EventHandler<CharacteristcEventArgs> CharacteristicRead = delegate { };
        public event EventHandler<CharacteristcEventArgs> CharacteristicChanged = delegate { };

        public List<BluetoothGattCharacteristic> Characteristics { get; set; }

        public bool IsConnected { get => ConnectedDevice != null; }

        public BluetoothClient()
        {
            Bluetooth = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;
            Manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
            Characteristics = new List<BluetoothGattCharacteristic>();
        }

        public ObservableCollection<IDevice> LoadConnectedOrPairedDevices()
        {
            return new ObservableCollection<IDevice>(Adapter.GetSystemConnectedOrPairedDevices());
        }

        public bool ConnectToDevice(IDevice device)
        {
            try
            {
                ConnectedDevice = null;
                if (Gatt != null) Gatt.Disconnect();

                Gatt = Manager.Adapter.GetRemoteDevice(((BluetoothDevice)device.NativeDevice).Address).ConnectGatt(Android.App.Application.Context, true, GattCallback);

                GattCallback.CharacteristicChanged += OnCharacteristicChanged;
                GattCallback.CharacteristicRead += OnCharacteristicRead;
                GattCallback.ConnectStateChanged += OnConnectionStateChanged;
                
                ConnectedDevice = device;

                return true;
            }
            catch (Exception ex)
            {
                Gatt = null;
                ConnectedDevice = null;
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public void Disconnect()
        {
            if (Gatt != null) Gatt.Disconnect();

            ConnectedDevice = null;
        }

        public IList<BluetoothGattService> GetServices()
        {
            if (!Gatt.DiscoverServices()) return null;
            return Gatt.Services;
        }

        public BluetoothGattService GetService(Java.Util.UUID uuid)
        {
            return Gatt.GetService(uuid);
        }

        public BluetoothGattCharacteristic SetupCharacteristic(BluetoothGattService service, Java.Util.UUID characteristcUuid, bool enableNotification = false)
        {
            BluetoothGattCharacteristic characteristic = service.GetCharacteristic(characteristcUuid);

            // Enable notifications for this characteristic locally -> read characteristic from server
            Gatt.SetCharacteristicNotification(characteristic, enableNotification);

            if (!Characteristics.Contains(characteristic)) Characteristics.Add(characteristic);

            return characteristic;
        }

        /// <summary>
        ///    BluetoothGattDescriptor.EnableNotificationValue
        /// </summary>
        /// <param name="characteristic"></param>
        /// <param name="descriptorUuid"></param>
        /// <param name="calue"></param>
        /// <returns></returns>
        public BluetoothGattDescriptor WriteDescriptor(BluetoothGattCharacteristic characteristic, Java.Util.UUID descriptorUuid, IList<byte> value)
        {
            BluetoothGattDescriptor descriptor = characteristic.GetDescriptor(descriptorUuid);
            byte[] config = new byte[value.Count];

            value.CopyTo(config, 0);

            descriptor.SetValue(config);
            Gatt.WriteDescriptor(descriptor);

            return descriptor;
        }

        public void OnCharacteristicRead(object sender, CharacteristcEventArgs characteristc)
        {
            if (!Characteristics.Any(x => x.Uuid.Equals(characteristc.CharacteristcsUuid))) return;

            CharacteristicRead(this, characteristc);
        }

        private void OnCharacteristicChanged(object sender, CharacteristcEventArgs characteristc)
        {
            if (!Characteristics.Any(x => x.Uuid.Equals(characteristc.CharacteristcsUuid))) return;

            CharacteristicChanged(this, characteristc);
        }

        public bool WriteCharacteristic(BluetoothGattCharacteristic characteristic, byte[] data)
        {
            characteristic.SetValue(data);
            return Gatt.WriteCharacteristic(characteristic);
        }

        private void OnConnectionStateChanged(object sender, CharacteristcEventArgs newState)
        {
            ConnectStateChanged(this, newState);
        }
    }
}
