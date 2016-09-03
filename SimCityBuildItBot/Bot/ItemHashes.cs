using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimCityBuildItBot.Bot
{
    public class ItemHashes
    {
        private List<PictureBox> pictureBoxes = new List<PictureBox>();
        private List<TextBox> textBoxes = new List<TextBox>();
        private Dictionary<ulong, string> hashes;

        private long folderNumber = 1;

        private string itemImagesPath = @"..\..\ItemImages";

        public ItemHashes(List<PictureBox> pictureBoxes, List<TextBox> textBoxes)
        {
            this.pictureBoxes = pictureBoxes;
            this.textBoxes = textBoxes;
        }

        public void ReadHashes()
        {
            CreateFolder(itemImagesPath);

            var directories = Directory.GetDirectories(itemImagesPath);

            hashes = new Dictionary<ulong, string>();

            // get existing folders
            directories.ToList().ForEach(dir =>
            {
                Directory.GetFiles(dir)
                .ToList()
                .ForEach(file =>
                {
                    var hash = file.Substring(file.LastIndexOf(@"\") + 1);
                    hash = hash.Substring(0, hash.Length - 4);
                    hashes.Add(ulong.Parse(hash), dir);
                }
                );
            });
        }

        private static void CreateFolder(string subPath)
        {
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
            }
        }

        public List<PanelLocation> ProcessCaptureImages(List<PanelLocation> panels)
        {
            var n = -1;

            this.pictureBoxes.ForEach(p =>
            {
                p.Visible = false;
            });

            this.textBoxes.ForEach(t => t.Text = "");

            panels.ForEach(panel =>
            {
                n++;
                if (this.pictureBoxes.Count > n)
                {
                    ImageViewer.ShowBitmap(panel.CroppedImage, this.pictureBoxes[n]);
                    this.pictureBoxes[n].Visible = true;
                }

                ulong hash = ImageHashing.ImageHashing.AverageHash(panel.CroppedImage);
                if (!hashes.ContainsKey(hash))
                {
                    var bestMatch = hashes.Keys.Where(key => ImageHashing.ImageHashing.Similarity(key, hash) > 90)
                        .OrderByDescending(key => ImageHashing.ImageHashing.Similarity(key, hash))
                        .FirstOrDefault();

                    var folder = string.Empty;

                    if (bestMatch == ulong.MinValue)
                    {
                        // new unknown image
                        folder = itemImagesPath + @"\F" + this.folderNumber;

                        // create a folder
                        while (Directory.Exists(folder))
                        {
                            folderNumber++;
                            folder = itemImagesPath + @"\F" + this.folderNumber;
                        }

                        CreateFolder(folder);
                    }
                    else
                    {
                        // matched image but a new hash
                        folder = hashes[bestMatch];
                    }

                    // save the image
                    var filename = folder + @"\" + hash + ".png";

                    if (!File.Exists(filename))
                    {
                        panel.CroppedImage.Save(filename, ImageFormat.Png);
                    }

                    // add the hash
                    hashes.Add(hash, folder);

                    //this.Text = DateTime.Now.ToLongTimeString() + " - found " + (bestMatch == ulong.MinValue ? "new" : "match") + " " + folder;

                    panel.CroppedImage.Dispose();
                    panel.CroppedImage = null;

                    panel.Item = folder;
                }
                else
                {
                    panel.Item = hashes[hash];
                }

                if (this.textBoxes.Count > n)
                {
                    this.textBoxes[n].Text = panel.ItemText;
                }
            });

            Application.DoEvents();

            return panels;
        }
    }
}
