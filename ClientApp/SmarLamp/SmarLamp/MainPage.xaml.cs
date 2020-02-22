using Android.Bluetooth;
using Plugin.BLE.Abstractions.Contracts;
using SmarLamp.Bluetooth;
using SmarLamp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Button = Xamarin.Forms.Button;
using Color = System.Drawing.Color;

namespace SmarLamp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Dictionary<Frame, string> PatternFrames { get; set; }
        private BluetoothClient BluetoothClient { get; set; }

        private Java.Util.UUID Service { get; } = Java.Util.UUID.FromString("795090c7-420d-4048-a24e-18e60180e23c");
        private Java.Util.UUID GetSettingsCharacteristic { get; } = Java.Util.UUID.FromString("31517c58-66bf-470c-b662-e352a6c80cba");
        private Java.Util.UUID GetSettingsConfigDescriptor { get; } = Java.Util.UUID.FromString("0b89d2d4-0ea6-4141-86bb-0c5fb91ab14a");
        private Java.Util.UUID SetSettingsCharacteristic { get; } = Java.Util.UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
        private LampData Data { get; set; }

        public MainPage()
        {
            InitializeComponent();

            LoadPatternFrames();
            InitBluetooth();
            Data = new LampData();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Yield();

            ShowBluetoothQuery();
            LoadDevices();
        }

        #region "Bluetooth"

        private void LoadDevices()
        {
            DevicePicker.ItemsSource = BluetoothClient.LoadConnectedOrPairedDevices();
        }

        private async void ShowBluetoothQuery()
        {
            // ask to activate bluetooth
            while (!BluetoothClient.Bluetooth.IsOn)
            {
                if (await DisplayAlert("Bluetooth is off", "Start bluetooth?", "Yes", "No"))
                {
                    if (BluetoothClient.Bluetooth.IsOn) return;

                    try
                    {
                        BluetoothClient.Manager.Adapter.Enable();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.ToString(), "Ok");
                    }

                }
                else
                {
                    if (BluetoothClient.Bluetooth.IsOn) return;
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void InitBluetooth()
        {
            BluetoothClient = new BluetoothClient();

            BluetoothClient.CharacteristicChanged += (s, ceo) =>
            {

            };

            BluetoothClient.CharacteristicRead += (s, ceo) =>
            {

            };

            BluetoothClient.ConnectStateChanged += (s, newStatus) =>
            {
                Device.BeginInvokeOnMainThread(() => 
                {
                 DisplayAlert("test",newStatus.NewState.ToString(), "Ok");
                });
            };

            BluetoothClient.Bluetooth.StateChanged += (s, e) => ShowBluetoothQuery();
        }

        private void DevicePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!BluetoothClient.ConnectToDevice((IDevice)DevicePicker.SelectedItem))
            {
                DisplayAlert("Error", "Could not connect to device.", "Ok");
                DevicePicker.SelectedItem = null;

                return;
            }

            try
            {
                BluetoothGattService service = BluetoothClient.GetService(Service);
                if (service is null) throw new Exception("Service not found");

                BluetoothClient.SetupCharacteristic(service, GetSettingsCharacteristic, true);
                BluetoothClient.WriteDescriptor(BluetoothClient.Characteristics.First(x => x.Uuid.Equals(GetSettingsCharacteristic)), GetSettingsConfigDescriptor, BluetoothGattDescriptor.EnableNotificationValue);

                BluetoothClient.SetupCharacteristic(service, SetSettingsCharacteristic);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Could not connect service or characteristic: " + ex.Message, "Ok");
                BluetoothClient.Disconnect();
                DevicePicker.SelectedItem = null;
            }

        }

        private void SendDataToServer()
        {
            if (BluetoothClient.IsConnected)
                BluetoothClient.WriteCharacteristic(BluetoothClient.Characteristics.First(x => x.Uuid.Equals(SetSettingsCharacteristic)), Data.ToByteArray());
        }

        #endregion

        #region "Pattern"

        private void LoadPatternFrames()
        {
            PatternFrames = new Dictionary<Frame, string>
            {
                { b0_0, "0-0" },
                { b0_1, "0-1" },
                { b0_2, "0-2" },
                { b0_3, "0-3" },
                { b0_4, "0-4" },
                { b0_5, "0-5" },
                { b0_6, "0-6" },
                { b0_7, "0-7" },

                { b1_0, "1-0" },
                { b1_1, "1-1" },
                { b1_2, "1-2" },
                { b1_3, "1-3" },
                { b1_4, "1-4" },
                { b1_5, "1-5" },
                { b1_6, "1-6" },
                { b1_7, "1-7" },

                { b2_0, "2-0" },
                { b2_1, "2-1" },
                { b2_2, "2-2" },
                { b2_3, "2-3" },
                { b2_4, "2-4" },
                { b2_5, "2-5" },
                { b2_6, "2-6" },
                { b2_7, "2-7" },

                { b3_0, "3-0" },
                { b3_1, "3-1" },
                { b3_2, "3-2" },
                { b3_3, "3-3" },
                { b3_4, "3-4" },
                { b3_5, "3-5" },
                { b3_6, "3-6" },
                { b3_7, "3-7" },

                { b4_0, "4-0" },
                { b4_1, "4-1" },
                { b4_2, "4-2" },
                { b4_3, "4-3" },
                { b4_4, "4-4" },
                { b4_5, "4-5" },
                { b4_6, "4-6" },
                { b4_7, "4-7" },

                { b5_0, "5-0" },
                { b5_1, "5-1" },
                { b5_2, "5-2" },
                { b5_3, "5-3" },
                { b5_4, "5-4" },
                { b5_5, "5-5" },
                { b5_6, "5-6" },
                { b5_7, "5-7" },

                { b6_0, "6-0" },
                { b6_1, "6-1" },
                { b6_2, "6-2" },
                { b6_3, "6-3" },
                { b6_4, "6-4" },
                { b6_5, "6-5" },
                { b6_6, "6-6" },
                { b6_7, "6-7" },

                { b7_0, "7-0" },
                { b7_1, "7-1" },
                { b7_2, "7-2" },
                { b7_3, "7-3" },
                { b7_4, "7-4" },
                { b7_5, "7-5" },
                { b7_6, "7-6" },
                { b7_7, "7-7" }
            };

            foreach (KeyValuePair<Frame, string> frame in PatternFrames)
            {
                frame.Key.IsEnabled = false;
            }
        }

        private void Button_Tapped(object sender, EventArgs e)
        {
            Frame frame = (Frame)sender;
            int x = int.Parse(PatternFrames[frame].Split('-')[0]);
            int y = int.Parse(PatternFrames[frame].Split('-')[1]);

            if (frame.BackgroundColor != Xamarin.Forms.Color.Black)
            {
                frame.BackgroundColor = Xamarin.Forms.Color.Black;
            }
            else
            {
                frame.BackgroundColor = CurrentColor.BackgroundColor;
            }

            if (!Data.Pattern.Any(p => p.X == x && p.Y == y))
            {
                Data.Pattern.Add(new Pixel() { X = x, Y = y, Color = new Models.Color() { R = (int)CurrentColor.BackgroundColor.R, G = (int)CurrentColor.BackgroundColor.G, B = (int)CurrentColor.BackgroundColor.B } });
            }
            else
            {
                Data.Pattern.First(p => p.X == x && p.Y == y).Color = new Models.Color() { R = (int)CurrentColor.BackgroundColor.R, G = (int)CurrentColor.BackgroundColor.G, B = (int)CurrentColor.BackgroundColor.B };
            }

            SendDataToServer();
        }

        #endregion

        private void LampOnOff(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.Text.Contains("ON"))
            {
                b.Text = "TURN OFF";
                Data.On = true;
            }
            else
            {
                b.Text = "TURN ON";
                Data.On = false;
            }

            SendDataToServer();
        }

        #region "Slider"

        private void Gamma_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            GammaLabel.Text = ((int)Gamma.Value).ToString();
            SetColorImage();
        }

        private void Red_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            RedLabel.Text = ((int)Red.Value).ToString();
            SetColorImage();
        }

        private void Green_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            GreenLabel.Text = ((int)Green.Value).ToString();
            SetColorImage();
        }

        private void Blue_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            BlueLabel.Text = ((int)Blue.Value).ToString();
            SetColorImage();
        }

        private void SetColorImage()
        {
            Data.Color = new Models.Color() { R = (int)Red.Value, G = (int)Green.Value, B = (int)Blue.Value };
            Data.Gamma = (int)Gamma.Value;

            CurrentColor.BackgroundColor = Color.FromArgb((int)Red.Value, (int)Green.Value, (int)Blue.Value);
            SendDataToServer();
        }

        #endregion

        private void UsePatternSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            foreach (KeyValuePair<Frame, string> frame in PatternFrames)
            {
                frame.Key.IsEnabled = e.Value;
            }

            Data.UsePattern = e.Value;

            SendDataToServer();
        }

    }

}
