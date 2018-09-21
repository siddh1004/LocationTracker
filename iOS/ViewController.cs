using System;
using CoreLocation;
using Plugin.DeviceInfo;
using Plugin.Media.Abstractions;
using UIKit;

namespace LocationTracker.iOS
{
    public partial class ViewController : UIViewController
    {
        int count = 1;
        public static LocationManager Manager { get; set; }

        public ViewController(IntPtr handle) : base(handle)
        {
            Manager = new LocationManager();
            Manager.StartLocationUpdates();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // It is better to handle this with notifications, so that the UI updates
            // resume when the application re-enters the foreground!
            Manager.LocationUpdated += HandleLocationChanged;

            var info = CrossDeviceInfo.Current;

            DeviceName.Text = info.DeviceName;
            DeviceVersion.Text = info.Model;
            DeviceOS.Text = info.Platform + " " + info.Version;
            DeviceUID.Text = info.Manufacturer;

            OpenCamera.TouchUpInside += async (s, e) =>
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
                }
                catch (Exception ex)
                {
                    //Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

            };

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        public static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
        {
            // Handle foreground updates
            CLLocation location = e.Location;

            LongitudeLabel.Text = location.Coordinate.Longitude.ToString();
            LatitudeLabel.Text = location.Coordinate.Latitude.ToString();

            Console.WriteLine("foreground updated");
        }
    }
}
