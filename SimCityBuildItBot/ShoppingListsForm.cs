using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimCityBuildItBot
{
    public partial class ShoppingListsForm : Form
    {
        public static string path = @"..\..\config";
        private static string suffix = ".sl.txt";
        private static string ItemImagesPath = @"..\..\ItemImages";

        private List<string> Items;

        public ShoppingListsForm()
        {
            InitializeComponent();

            RefreshShoppingLists();

            Items = Directory.GetDirectories(ItemImagesPath).ToList();
        }

        private void RefreshShoppingLists()
        {
            listBoxShoppingList.Items.Clear();
            GetShoppingLists()
               .ForEach(file =>
               {
                   listBoxShoppingList.Items.Add(file.Substring(file.LastIndexOf(@"\")+1));
               });
        }

        public static List<string> GetShoppingLists()
        {
            return Directory.GetFiles(path)
               .Where(file => file.EndsWith(suffix)).ToList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            File.Delete(path + @"\" + this.listBoxShoppingList.SelectedItem.ToString());
        }

        private void btnAddShoppingList_Click(object sender, EventArgs e)
        {
            StreamWriter file = new StreamWriter(path + @"\" + this.txtShoppingListName.Text+ suffix);
            file.Close();
            RefreshShoppingLists();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (this.listBoxSelected.SelectedItem==null)
            {
                return;
            }

            var deleteItem = this.listBoxSelected.SelectedItem.ToString();
            var items = File.ReadAllLines(GetSelectedFilePath())
                .Where(s => s != deleteItem)
                .Distinct();

            File.Delete(GetSelectedFilePath());
            File.WriteAllLines(GetSelectedFilePath(), items);
            listBoxShoppingList_SelectedIndexChanged(sender, e);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (this.listBoxAvailable.SelectedItem == null)
            {
                return;
            }

            var newItem = this.listBoxAvailable.SelectedItem.ToString();
            File.AppendAllLines(GetSelectedFilePath(), new List<string> { newItem });

            listBoxShoppingList_SelectedIndexChanged(sender, e);
        }

        private string GetSelectedFilePath()
        {
            var filename = this.listBoxShoppingList.SelectedItem.ToString();
            return path + @"\" + filename;
        }

        private void listBoxShoppingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItems = File.ReadAllLines(GetSelectedFilePath()).ToList();

            this.listBoxSelected.Items.Clear();
            selectedItems
                .ForEach(item => this.listBoxSelected.Items.Add(item));

            this.listBoxAvailable.Items.Clear();
            Items.Select(item=> item.Substring(item.LastIndexOf(@"\") + 1))
                .Where(i => !selectedItems.Contains(i))
                .OrderBy(s => s)
                .ToList()
                .ForEach(item => this.listBoxAvailable.Items.Add(item));
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var items = File.ReadAllLines(GetSelectedFilePath()).ToArray();

            var index = this.listBoxSelected.SelectedIndex;
            if (index < 1)
            {
                return;
            }

            var oldItem = items[index - 1];
            items[index - 1] = items[index];
            items[index] = oldItem;

            SaveAndReloadSelectedItems(items);
        }

        private void SaveAndReloadSelectedItems(string[] items)
        {
            File.Delete(GetSelectedFilePath());
            File.WriteAllLines(GetSelectedFilePath(), items);

            var selectedItems = File.ReadAllLines(GetSelectedFilePath()).ToList();
            this.listBoxSelected.Items.Clear();
            selectedItems
                .ForEach(item => this.listBoxSelected.Items.Add(item));
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var items = File.ReadAllLines(GetSelectedFilePath()).ToArray();

            var index = this.listBoxSelected.SelectedIndex;
            if (index >= items.Length-1)
            {
                return;
            }

            var oldItem = items[index + 1];
            items[index + 1] = items[index];
            items[index] = oldItem;

            SaveAndReloadSelectedItems(items);
        }
    }
}
