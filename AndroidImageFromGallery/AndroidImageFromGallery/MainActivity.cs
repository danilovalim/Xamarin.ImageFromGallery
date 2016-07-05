using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Database;
using Android.Provider;

namespace AndroidImageFromGallery
{
    [Activity(Label = "AndroidImageFromGallery", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static readonly int PickImageId = 1000;
        private string pathToImage = "";
        private ImageView imgPic;
        private Button mButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mButton = FindViewById<Button>(Resource.Id.MyButton);
            imgPic = FindViewById<ImageView>(Resource.Id.imgPic);

            mButton.Click += MButton_Click;
        }

        private void MButton_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Android.Net.Uri uri;

            if (resultCode == Result.Ok)
            {
                uri = data.Data;
                imgPic.SetImageURI(uri);

                pathToImage = GetPathToImage(uri);

                Toast.MakeText(this, pathToImage.ToString(), ToastLength.Short).Show();
            }
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            ICursor cursor = this.ContentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string document_id = cursor.GetString(0);
            if (document_id.Contains(":"))
                document_id = document_id.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { document_id }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }
    }
}

