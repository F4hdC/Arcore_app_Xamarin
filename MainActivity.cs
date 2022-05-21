using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Google.AR.Sceneform.UX;
using Google.AR.Sceneform.Rendering;
using Google.AR.Core;
using Google.AR.Sceneform;
using Java.Util.Concurrent;

namespace Arcore_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ArFragment arFragment;
        private Material readyColor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            
            arFragment = (ArFragment)this.SupportFragmentManager.FindFragmentById(Resource.Id.fragmentAR);
            setUpPlane();
            MaterialFactory.MakeOpaqueWithColor(this, new Color(Android.Graphics.Color.Green)).GetAsync().ContinueWith(materialTask =>
            {
                readyColor = (Material)materialTask.Result;
            });
        }

        private void setUpPlane()
        {
            arFragment.TapArPlane += (sender, args) => this.OnTapArPlaneListener(args.HitResult, args.Plane, args.MotionEvent);
        }

        private void OnTapArPlaneListener(HitResult hitResult, Plane plane, MotionEvent motionEvent)
        {
            Anchor anchor = hitResult.CreateAnchor();
            AnchorNode anchorNode = new AnchorNode(anchor);
            anchorNode.SetParent(arFragment.ArSceneView.Scene);
            createModel(anchorNode);
        }

        private void createModel(AnchorNode anchorNode)
        {
            Google.AR.Sceneform.UX.TransformableNode node = new TransformableNode(arFragment.TransformationSystem);
            node.SetParent(anchorNode);
            var nodeRender = ShapeFactory.MakeSphere(0.07f, new Google.AR.Sceneform.Math.Vector3(0.0f, 0.15f, 0.0f), readyColor);
            node.Renderable = nodeRender;
            node.Select();
        }

     

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
