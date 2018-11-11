using ScraBoy.Features.Product;
using ScraBoy.Features.Data;
using ScraBoy.Features.Shop;
using ScraBoy.Features.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Inventory.Data
{
    public class InventroySeed
    {
        public async static Task SeedDataAsync()
        {
            using(var db = new CMSContext ())
            {
                var memory = new TypeModel { TypeName = "Memory" };
                var cpu = new TypeModel { TypeName = "Cpu" };
                var hdd = new TypeModel { TypeName = "Hdd" };

                db.Type.AddRange(new TypeModel[] { memory,cpu,hdd });

                var iconComp = new ShopModel { ShopName = "Icon Comp",ShopLink = "https://www.tokopedia.com/iconcomp" };
                var hsComp = new ShopModel { ShopName = "HS Comp",ShopLink = "https://www.tokopedia.com/hs-com" };
                var twentyOne = new ShopModel { ShopName = "Twenty one21",ShopLink = "https://www.tokopedia.com/twenty21" };

                db.Shop.AddRange(new ShopModel[] { iconComp,hsComp,twentyOne });


                db.Product.Add(new ProductModel
                {
                        ProductName = "SSD WD 120GB  SATA 3",
                        UnitPrice = 500000,
                        ShipmentPrice = 11000,
                        Type = hdd,
                        Shop = hsComp,
                        ExpiresOn = new DateTime(2018,10,3),
                        Url = "https://www.tokopedia.com/hs-com/asus-z97-sabertooth-mark-ii-usb-3-1?trkid=f=Ca0000L000P0W0S0Sh00Co0Po0Fr0Cb0_src=shop-product_page=1_ob=11_q=_po=4_catid=302&lt=/shoppage+-+product+1+-+product+-+All+Showcase"
                });

                db.Product.Add(new ProductModel
                {
                    ProductName = "I5 4690",
                    UnitPrice = 2000000,
                    ShipmentPrice = 15000,
                    Type = cpu,
                    Shop = iconComp,
                    ExpiresOn = new DateTime(2018,10,3),
                    Url = "https://www.tokopedia.com/hs-com/asus-z97-sabertooth-mark-ii-usb-3-1?trkid=f=Ca0000L000P0W0S0Sh00Co0Po0Fr0Cb0_src=shop-product_page=1_ob=11_q=_po=4_catid=302&lt=/shoppage+-+product+1+-+product+-+All+Showcase"
                });

                db.Product.Add(new ProductModel
                {
                    ProductName = "I5 4670",
                    UnitPrice = 1900000,
                    ShipmentPrice = 50000,
                    Type = cpu,
                    Shop = hsComp,
                    ExpiresOn = new DateTime(2018,10,3),
                    Url = "https://www.tokopedia.com/hs-com/asus-z97-sabertooth-mark-ii-usb-3-1?trkid=f=Ca0000L000P0W0S0Sh00Co0Po0Fr0Cb0_src=shop-product_page=1_ob=11_q=_po=4_catid=302&lt=/shoppage+-+product+1+-+product+-+All+Showcase"
                });

                db.Product.Add(new ProductModel
                {
                    ProductName = "Motherboard Gigabyte H81M-S1 Rev2.1",
                    UnitPrice = 560000,
                    ShipmentPrice = 11000,
                    Type = memory,
                    Shop = twentyOne,
                    ExpiresOn = new DateTime(2018,10,3),
                    Url = "https://www.tokopedia.com/hs-com/asus-z97-sabertooth-mark-ii-usb-3-1?trkid=f=Ca0000L000P0W0S0Sh00Co0Po0Fr0Cb0_src=shop-product_page=1_ob=11_q=_po=4_catid=302&lt=/shoppage+-+product+1+-+product+-+All+Showcase"
                });

                db.Product.Add(new ProductModel
                {
                    ProductName = "Seasonic S12II-450 EVO 450W",
                    UnitPrice = 650000,
                    ShipmentPrice = 11000,
                    Type = hdd,
                    Shop = hsComp,
                    ExpiresOn = new DateTime(2018,10,3),
                    Url = "https://www.tokopedia.com/hs-com/asus-z97-sabertooth-mark-ii-usb-3-1?trkid=f=Ca0000L000P0W0S0Sh00Co0Po0Fr0Cb0_src=shop-product_page=1_ob=11_q=_po=4_catid=302&lt=/shoppage+-+product+1+-+product+-+All+Showcase"
                });

                await db.SaveChangesAsync();
            }
        }
    }
}