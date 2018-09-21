using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using Android.Runtime;
using Plugin.DeviceInfo;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Hardware;
using System.IO;
using Plugin.CurrentActivity;
using System;
using Plugin.Media.Abstractions;
using Android.Graphics;

namespace LocationTracker.Droid
{
    [Activity(Label = "LocationTracker", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        Android.Locations.Location currentLocation;
        LocationManager locationManager;
        string locationProvider;
        TextView Latitude;
        TextView Longitude;
        TextView DeviceName;
        TextView DeviceOs;
        TextView DeviceVersion;
        TextView DeviceUid;
        Button OpenCamera;
        int CameraOpenRequest = 101;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Latitude = FindViewById<TextView>(Resource.Id.LatitudeTextView);
            Longitude = FindViewById<TextView>(Resource.Id.LongitudeTextView);
            DeviceName = FindViewById<TextView>(Resource.Id.DeviceNameTextView);
            DeviceOs = FindViewById<TextView>(Resource.Id.DeviceOSTextView);
            DeviceVersion = FindViewById<TextView>(Resource.Id.DeviceVersionTextView);
            DeviceUid = FindViewById<TextView>(Resource.Id.DeviceUidTextView);
            OpenCamera = FindViewById<Button>(Resource.Id.OpenCameraButton);

            OpenCamera.Click += async(s, e) =>
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


            var info = CrossDeviceInfo.Current;
            DeviceName.Text = info.DeviceName;
            DeviceOs.Text = info.Model;
            DeviceVersion.Text = info.Platform + " " + info.Version;
            DeviceUid.Text = info.Manufacturer;
            InitializeLocationManager();

        }

        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }
        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }
            //Log.Debug(Tag, "Using " + locationProvider + ".");
        }


        public void OnLocationChanged(Android.Locations.Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
                //Error Message  
            }
            else
            {
                Latitude.Text = currentLocation.Latitude.ToString();
                Longitude.Text = currentLocation.Longitude.ToString();
            }
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}

