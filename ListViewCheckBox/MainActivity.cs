using Android.App;
using Android.Widget;
using Android.OS;
using Java.Interop;
using Android.Views;
using System;
using System.Collections.Generic;

namespace ListViewCheckBox
{
    [Activity(Label = "ListViewCheckBox", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public class Item
        {
            public string Name { get; set; }
            public bool Checked { get; set; }
        }

        List<Item> Items;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            CreateItems();
        }

        private void CreateItems()
        {
            Items = new List<Item>();

            for (int i = 0; i < 30; i++)
            {
                Item Item = new Item();
                Item.Name = "Question " + i.ToString();
                Items.Add(Item);
            }
        }

        [Export("onButtonClicked")]
        public void onButtonClicked(View view)
        {
            switch (view.Id)
            {
                case Resource.Id.btnShowDialog:
                    ShowDialog();
                    break;
            }
        }

        private void ShowDialog()
        {
            AlertDialog dlgList = (new AlertDialog.Builder(this)).Create();
            dlgList.SetTitle("Questions");
            var viewAD = this.LayoutInflater.Inflate(Resource.Layout.DialogList, null);

            ListView lvItems = viewAD.FindViewById<ListView>(Resource.Id.lvDialog);

            QuestionAdapter adItems = new QuestionAdapter(Items, this);
            lvItems.Adapter = adItems;

            dlgList.SetView(viewAD);
            dlgList.SetButton("Close", delegate { });
            dlgList.Show();
        }

        public class QuestionAdapter : BaseAdapter
        {
            private Activity context;
            private List<Item> _Items;

            public QuestionAdapter(List<Item> Questions, Activity context)
            {
                this.context = context;
                _Items = Questions;
            }

            // How many items are in the data set represented by this Adapter.
            public override int Count
            {
                get { return _Items.Count; }
            }

            // Get the data item associated with the specified position in the data set.
            public override Java.Lang.Object GetItem(int position)
            {
                return position;
            }

            // Get the row id associated with the specified position in the list.
            public override long GetItemId(int position)
            {
                return position;
            }

            // Get a View that displays the data at the specified position in the data set.
            // You can either create a View manually or inflate it from an XML layout file.
            public override View GetView(int position, View convertView, ViewGroup parent)
            {

                Item Item = _Items[position];

                if (convertView == null)
                {
                    convertView = context.LayoutInflater.Inflate(Resource.Layout.DialogCheckListItem, null);
                }

                TextView t = convertView.FindViewById<TextView>(Resource.Id.txtItem);
                t.Text = Item.Name;

                CheckBox c = convertView.FindViewById<CheckBox>(Resource.Id.chkItem);
                c.Checked = Item.Checked;
                c.CheckedChange += (s, e) =>
                {
                    Item.Checked = e.IsChecked;
                };

                return convertView;
            }
        }
    }
}

