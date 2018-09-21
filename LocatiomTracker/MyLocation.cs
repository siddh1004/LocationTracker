using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace LocatiomTracker
{
    public class MyLocation
    {
        private static Action<Position> _position;

        public MyLocation(Action<Position> position)
        {
            _position = position;
        }

        public async Task<Position> GetCurrentLocation()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Unable to get location: " + ex);
            }
            return position;
        }

        public async Task StartListening()
        {
            if (CrossGeolocator.IsSupported &&
                CrossGeolocator.Current.IsGeolocationAvailable &&
                CrossGeolocator.Current.IsGeolocationEnabled)
            {
                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new ListenerSettings
                {
                    ActivityType = ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = true,
                    DeferralDistanceMeters = 1,
                    DeferralTime = TimeSpan.FromSeconds(1),
                    ListenForSignificantChanges = true,
                    PauseLocationUpdatesAutomatically = false
                });

                CrossGeolocator.Current.PositionChanged += PositionChanged;
            }
        }

        public static async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
        }

        private static void PositionChanged(object sender, PositionEventArgs e)
        {
            _position.Invoke(e.Position);
        }
    }
}