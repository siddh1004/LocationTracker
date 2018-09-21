// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LocationTracker.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton Button { get; set; }

		[Outlet]
		UIKit.UILabel DeviceName { get; set; }

		[Outlet]
		UIKit.UILabel DeviceOS { get; set; }

		[Outlet]
		UIKit.UILabel DeviceUID { get; set; }

		[Outlet]
		UIKit.UILabel DeviceVersion { get; set; }

		[Outlet]
		UIKit.UILabel LatitudeLabel { get; set; }

		[Outlet]
		UIKit.UILabel LongitudeLabel { get; set; }

		[Outlet]
		UIKit.UIButton OpenCamera { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Button != null) {
				Button.Dispose ();
				Button = null;
			}

			if (DeviceName != null) {
				DeviceName.Dispose ();
				DeviceName = null;
			}

			if (DeviceOS != null) {
				DeviceOS.Dispose ();
				DeviceOS = null;
			}

			if (DeviceUID != null) {
				DeviceUID.Dispose ();
				DeviceUID = null;
			}

			if (DeviceVersion != null) {
				DeviceVersion.Dispose ();
				DeviceVersion = null;
			}

			if (LatitudeLabel != null) {
				LatitudeLabel.Dispose ();
				LatitudeLabel = null;
			}

			if (LongitudeLabel != null) {
				LongitudeLabel.Dispose ();
				LongitudeLabel = null;
			}

			if (OpenCamera != null) {
				OpenCamera.Dispose ();
				OpenCamera = null;
			}
		}
	}
}
