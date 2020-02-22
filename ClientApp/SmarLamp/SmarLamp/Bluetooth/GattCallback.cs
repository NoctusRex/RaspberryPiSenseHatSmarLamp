using System;
using System.Threading.Tasks;
using Android.Bluetooth;

namespace SmarLamp.Bluetooth
{
    public class GattCallback : BluetoothGattCallback
    {

        public event EventHandler<CharacteristcEventArgs> ConnectStateChanged = delegate { };

        public event EventHandler<CharacteristcEventArgs> CharacteristicRead = delegate { };

        public event EventHandler<CharacteristcEventArgs> CharacteristicChanged = delegate { };

        /// <Docs>GATT client</Docs>
        /// <summary>
        /// Raises the connection state change event.
        /// </summary>
        /// <param name="gatt">Gatt.</param>
        /// <param name="status">Status.</param>
        /// <param name="newState">New state.</param>
        public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            base.OnConnectionStateChange(gatt, status, newState);

            ConnectStateChanged(this, new CharacteristcEventArgs(null, null, newState));
        }

        /// <summary>
        /// Raises the characteristic read event.
        /// </summary>
        /// <param name="gatt">Gatt.</param>
        /// <param name="characteristic">Characteristic.</param>
        /// <param name="status">Status.</param>
        public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            base.OnCharacteristicRead(gatt, characteristic, status);

            if (status != GattStatus.Success) return;

            CharacteristicRead(this, new CharacteristcEventArgs(characteristic.Uuid, characteristic.GetValue(), ProfileState.Connected));
        }

        /// <Docs>GATT client the characteristic is associated with</Docs>
        /// <summary>
        /// Callback triggered as a result of a remote characteristic notification.
        /// </summary>
        /// <para tool="javadoc-to-mdoc">Callback triggered as a result of a remote characteristic notification.</para>
        /// <format type="text/html">[Android Documentation]</format>
        /// <since version="Added in API level 18"></since>
        /// <param name="gatt">Gatt.</param>
        /// <param name="characteristic">Characteristic.</param>
        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            base.OnCharacteristicChanged(gatt, characteristic);

            CharacteristicChanged(this, new CharacteristcEventArgs(characteristic.Uuid, characteristic.GetValue(), ProfileState.Connected));
        }

    }

}
