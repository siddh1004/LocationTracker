using Android.App;
using Android.Widget;
using Android.OS;
using Plugin.DeviceInfo;
using Plugin.CurrentActivity;
using System;
using System.Globalization;
using System.Threading.Tasks;
using LocatiomTracker;
using Plugin.Geolocator.Abstractions;
using Plugin.Media.Abstractions;

namespace LocationTracker.Droid
{
    [Activity(Label = "LocationTracker", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private TextView _latitude;
        private TextView _longitude;
        private TextView _deviceName;
        private TextView _deviceOs;
        private TextView _deviceVersion;
        private TextView _deviceUid;
        private Button _openCamera;
        private Position _position;
        private MyLocation _location;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            _location = new MyLocation(SetLocation);
            await _location.StartListening();

            FindViewById();

            SetEventHandlers();
            SetDeviceInfo();

            await GetLocation();
        }

        public async Task GetLocation()
        {
            _position = await _location.GetCurrentLocation();
            SetLocation(_position);
        }

        private void SetLocation(Position position)
        {
            if (position != null)
            {
                _latitude.Text = position.Latitude.ToString(CultureInfo.InvariantCulture);
                _longitude.Text = position.Longitude.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void SetDeviceInfo()
        {
            var info = CrossDeviceInfo.Current;
            _deviceName.Text = info.DeviceName;
            _deviceOs.Text = info.Model;
            _deviceVersion.Text = info.Platform + " " + info.Version;
            _deviceUid.Text = info.Manufacturer;
        }

        private void FindViewById()
        {
            _latitude = FindViewById<TextView>(Resource.Id.LatitudeTextView);
            _longitude = FindViewById<TextView>(Resource.Id.LongitudeTextView);
            _deviceName = FindViewById<TextView>(Resource.Id.DeviceNameTextView);
            _deviceOs = FindViewById<TextView>(Resource.Id.DeviceOSTextView);
            _deviceVersion = FindViewById<TextView>(Resource.Id.DeviceVersionTextView);
            _deviceUid = FindViewById<TextView>(Resource.Id.DeviceUidTextView);
            _openCamera = FindViewById<Button>(Resource.Id.OpenCameraButton);
        }

        private void SetEventHandlers()
        {
            _openCamera.Click += async (s, e) =>
            {
                try
                {
                    var size = PhotoSize.Full;
                    var media = new Plugin.Media.MediaImplementation();
                    var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = $"{DateTime.Now}_{size}|\\?*<\":>/'.jpg".Replace(" ", string.Empty),
                        SaveToAlbum = true,
                        PhotoSize = PhotoSize.Full,
                        DefaultCamera = CameraDevice.Front
                    });

                    if (file == null)
                        return;
                    var path = file.Path;
                    Toast.MakeText(this, path, ToastLength.Long).Show();
                    System.Diagnostics.Debug.WriteLine(path);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

            };
        }

        public override void OnRequestPermissionsResult(
            int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

