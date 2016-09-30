using Common.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace SimCityBuildItBot.Bot
{
    public class Salesman
    {
        private Touch touch;
        private TradeWindow tradeWindow;
        private TradePanelCapture tradePanelCapture;
        private ItemHashes itemHashes;
        private NavigateToBuilding navigateToBuilding;
        private ILog log;

        private int swipeSteps = 10;

        public enum SaleResult
        {
            Other,
            SoldItem,
            NothingToSell,
            NoSpace
        }


        public Salesman(Touch touch,
         TradeWindow tradeWindow,
         TradePanelCapture tradePanelCapture,
         ItemHashes itemHashes,
         NavigateToBuilding navigateToBuilding, ILog log)
        {
            this.touch = touch;
            this.tradeWindow = tradeWindow;
            this.tradePanelCapture = tradePanelCapture;
            this.itemHashes = itemHashes;
            this.navigateToBuilding = navigateToBuilding;
            this.log = log;
        }

        public SaleResult SellAnItem(out string itemSold)
        {
            itemSold = string.Empty;

            for (int i = 0; i < 3; i++)
            {
                CollectSales();

                var result = PutItemOnSale(out itemSold);
                if (result == SaleResult.SoldItem || result== SaleResult.NothingToSell)
                {
                    return result;
                }

                if (i < 2)
                {
                    // move to next panel
                    touch.Swipe(Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleRight, Bot.Location.GlobalTradeMiddleLeft, swipeSteps, true);
                }
            }

            return SaleResult.Other;
        }

        private SaleResult PutItemOnSale(out string itemSold)
        {
            itemSold = string.Empty;

            // get images of trade boxes
            var panels = itemHashes.ProcessCaptureImages(CaptureImagesTradeDepot());

            // find an empty box
            var newSale = panels.Where(d => d.Item.ToLower().Contains("TradeDepotCreateNewSale".ToLower())).FirstOrDefault();
            if (newSale == null)
            {
                return SaleResult.NoSpace;
            }

            // click on the box
            var clickPoint = this.tradeWindow.CalcClickPointTradeDepot(newSale);
            touch.ClickAt(clickPoint);
            Debug.WriteLine(DateTime.Now.ToShortTimeString() + " opened sale point");
            BotApplication.Wait(1000);

            // check that the create sale window is visible
            if (!tradeWindow.IsEditSaleCloseButtonVisible())
            {
                return SaleResult.Other;
            }

            // find an item to sell from the create sale inventory
            var saleItems = this.tradePanelCapture.CaptureCreateSaleItems();
            var saleItem = GetItemToSell(saleItems);
            if (saleItem == null)
            {
                log.Debug("Nothing to sell" );
                return SaleResult.NothingToSell;
            }

            // click on the item
            clickPoint = this.tradeWindow.CalcClickPointCreateSaleItem(saleItem);
            touch.ClickAt(clickPoint);

            // long press on the item number button
            touch.LongPress(Bot.Location.CreateSaleQuantityPlus, 3);

            // long press on the item value button
            touch.LongPress(Bot.Location.CreateSalePricePlus, 3);

            // put it on sale
            touch.ClickAt(Bot.Location.CreateSalePutOnSale);

            Debug.WriteLine(DateTime.Now.ToShortTimeString() + " Sold: " + saleItem.Item);
            log.Debug("Sold: " + saleItem.Item);

            itemSold = saleItem.Item;
            return SaleResult.SoldItem;
        }

        private PanelLocation GetItemToSell(List<PanelLocation> panels)
        {
            //var itemsToSell = new List<string> { "vegetables","metal","nails", "chairs",  "flour", "watch", "hammer", "donut", "glass", "chemicals", "gardenfurniture", "shoes","grass","bricks","cement","cap","table","icecremesandwich", "greenSmoothie" };
            var itemsToSell = new List<string> {  "metal", "nails","wood","hammer" };
            var items = itemHashes.ProcessCaptureImages(panels);

            foreach (var item in items)
            {
                var match = itemsToSell.Where(itemToSell => item.Item.ToLower().Contains(itemToSell)).FirstOrDefault();
                if (match != null)
                {
                    return item;
                }
            }

            return null;
        }

        public void CollectSales()
        {
            var panels = itemHashes.ProcessCaptureImages(CaptureImagesTradeDepot());

            var soldItems = panels.Where(d => d.Item.ToLower().Contains("TradeDepotSoldItem".ToLower())).ToList();
            soldItems.ForEach(sold =>
            {
                var clickPoint = this.tradeWindow.CalcClickPointTradeDepot(sold);
                touch.ClickAt(clickPoint);
                Debug.WriteLine(DateTime.Now.ToShortTimeString() + " Collected sale");
                BotApplication.Wait(1000);
            });
        }

        public List<PanelLocation> CaptureImagesTradeDepot()
        {
            var image = this.tradeWindow.CaptureTradeWindow();

            var tops = this.tradeWindow.imageTopsTradeDepot;

            var locs = tradePanelCapture.FindPanelStartsTradeDepot(image, tops);

            locs.ForEach(p =>
            {
                p.CroppedImage = cropImage(image, new Rectangle(p.ImageTradeDepotPoint, p.ImageTradeDepotSize));
            });

            return locs;
        }

        private static Bitmap cropImage(Bitmap bmpImage, Rectangle cropArea)
        {
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public SaleResult Sell(out string itemSold)
        {
            itemSold = string.Empty;

            navigateToBuilding.NavigateTo(BuildingMatch.Get(Building.TradeDepot), 1);

            if (!this.tradeWindow.IsTradeDepotLogoVisible())
            {
                return SaleResult.Other;
            }

            var result = this.SellAnItem(out itemSold);

            touch.ClickAt(Bot.Location.HomeButton);

            return result;
        }
    }
}