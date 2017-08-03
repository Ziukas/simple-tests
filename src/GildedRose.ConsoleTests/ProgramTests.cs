using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRose.Console;
using System.Collections.Generic;


namespace GildedRose.Console.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        /*- All items have a SellIn value which denotes the number of days we have 
            to sell the item
            +++- All items have a Quality value which denotes how valuable the item is
            +++- At the end of each day our system lowers both values for every item

            +++- Once the sell by date has passed, Quality degrades twice as fast
            +++- The Quality of an item is never negative
            +++- "Aged Brie" actually increases in Quality the older it gets
            +++- The Quality of an item is never more than 50 
            (however "Sulfuras" is a legendary item and as such its  Quality is 80 and it never alters.)
            +++- "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
            +++- "Backstage passes", like aged brie, increases in Quality as it's SellIn 
            value approaches; Quality increases by 2 when there are 10 days or less 
            and by 3 when there are 5 days or less but Quality drops to 0 after the 
            concert

            We have recently signed a supplier of conjured items. This requires an 
            update to our system:

            +++- "Conjured" items degrade in Quality twice as fast as normal items

            Feel free to make any changes to the UpdateQuality method and add any 
            new code as long as everything still works correctly. However, do not 
            alter the Item class or Items property as those belong to the goblin 
            in the corner who will insta-rage and one-shot you as he doesn't 
            believe in shared code ownership (you can make the UpdateQuality 
            method and Items property static if you like, we'll cover for you).
           
            */


        //At the end of each day our system lowers both values for every item (also covers items having properties)
        [TestMethod()]
        public void UpdateQualityTest_RegularExample()
        {
            List<Item> Items = new List<Item>{                
                new Item{Name    = "RegularItem",
                         SellIn  = 10,
                         Quality = 49}
            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();

            Assert.AreEqual(app.Items[0].Name, "RegularItem");
            Assert.AreEqual(app.Items[0].SellIn, 9);
            Assert.AreEqual(app.Items[0].Quality, 48);
        }

        //- Once the sell by date has passed, Quality degrades twice as fast
        [TestMethod()]
        public void UpdateQualityTest_QualityDegradesTwiceAsFast()
        {
            List<Item> Items = new List<Item>{
                new Item{Name    = "RegularItem",
                         SellIn  = 0,
                         Quality = 44}
            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();

            Assert.AreEqual(app.Items[0].SellIn, -1);
            Assert.AreEqual(app.Items[0].Quality, 42);
        }

        // The Quality of an item is never negative 
        [TestMethod()]
        public void UpdateQualityTest_NegativeExample()
        {
            List<Item> Items = new List<Item>{
                new Item{Name    = "RegularItem", SellIn  = 5, Quality = 0},
                new Item{Name    = "Negative SellIn Item", SellIn  = -1, Quality = 1},  //will try to get -2
            };


            Program app = new Program { Items = Items };
            app.UpdateQuality();
            Assert.AreEqual(app.Items[0].Quality, 0);
            Assert.AreEqual(app.Items[1].Quality, 0);
        }

        // "Aged Brie" actually increases in Quality the older it gets
        [TestMethod()]
        public void UpdateQualityTest_RegularAgedBrie()
        {
            List<Item> Items = new List<Item>{
                new Item{Name    = "Aged Brie", SellIn  = 8, Quality = 5}
            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();
         
            Assert.AreEqual(app.Items[0].SellIn, 7);
            Assert.AreEqual(app.Items[0].Quality, 6);
        }

        // "Aged Brie" actually increases in Quality the older it gets 
        //Instead of quality getting reduced at double pace when SellIn date comes, it starts increasing at double speed (as in the starting method)
        [TestMethod()]
        public void UpdateQualityTest_SellInExpiredAgedBrie()
        {
            List<Item> Items = new List<Item>{
                new Item{Name    = "Aged Brie", SellIn  = 0, Quality = 5}
            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();

            Assert.AreEqual(app.Items[0].SellIn, -1);
            Assert.AreEqual(app.Items[0].Quality, 7);
        }

        //- "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        [TestMethod()]
        public void UpdateQualityTest_SulfurasExample()
        {
            List<Item> Items = new List<Item>{
                new Item{Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 },                
            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();
                        
            Assert.AreEqual(app.Items[0].SellIn, 0);
            Assert.AreEqual(app.Items[0].Quality, 80);            
        }

        //- "Backstage passes", like aged brie, increases in Quality as it's SellIn 
        //value approaches; Quality increases by 2 when there are 10 days or less
        //and by 3 when there are 5 days or less but Quality drops to 0 after the  concert
        [TestMethod()]
        public void UpdateQualityTest_BackstagePass()
        {
            List<Item> Items = new List<Item>{
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 },
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 },
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 },
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 15 },
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = -1, Quality = 15 },
            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();

            Assert.AreEqual(app.Items[0].Quality, 21);
            Assert.AreEqual(app.Items[1].Quality, 22);
            Assert.AreEqual(app.Items[2].Quality, 23);
            Assert.AreEqual(app.Items[3].Quality, 0);
            Assert.AreEqual(app.Items[4].Quality, 0);
        }

        //- The Quality of an item is never more than 50
        [TestMethod()]
        public void UpdateQualityTest_IncreasingQualityItemsCannotClimbOver50()
        {
            List<Item> Items = new List<Item>{
                new Item{Name = "Aged Brie", SellIn = 5, Quality = 50 }, //will try to get +1
                new Item{Name = "Aged Brie", SellIn = 0, Quality = 49 }, //will try to get +2
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 49 }, //will try to get +2
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 48 },  //will try to get +3
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 49 },  //will try to get +3
                new Item{Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 50 },  //will try to get +3
                new Item{Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 }, //the one exception                
            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();

            Assert.AreEqual(app.Items[0].Quality, 50);
            Assert.AreEqual(app.Items[1].Quality, 50);
            Assert.AreEqual(app.Items[2].Quality, 50);
            Assert.AreEqual(app.Items[3].Quality, 50);
            Assert.AreEqual(app.Items[4].Quality, 50);
            Assert.AreEqual(app.Items[5].Quality, 50);
            Assert.AreEqual(app.Items[6].Quality, 80);
            
        }

        //new functionality, test written before impimentation
        //- "Conjured" items degrade in Quality twice as fast as normal items
        //Assumptions: 
            //- Conjured items do not follow special items' rules (Conjured Backstage passes, Conjured Sulfuras,Conjured Aged Brie all function as regular Conjured items)
            //- Conjured items whose SellIn date has passed should decay four times as fast        
        //Conjured items threaten rule about quality never falling bellow 0, need to verify it too (items 4 and 5)
        [TestMethod()]
        public void UpdateQualityTest_ConjuredItems()
        {
            List<Item> Items = new List<Item>{
                new Item{Name = "Conjured Aged Brie", SellIn = 5, Quality = 10 }, 
                new Item{Name = "Conjured Sulfuras", SellIn = -1, Quality = 20 }, 
                new Item{Name = "Conjured Regural Item", SellIn = 10, Quality = 10 }, 
                new Item{Name = "Conjured Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 10 },
                new Item{Name = "Conjured Item-that-must-not-degrade-past-zero", SellIn = -1, Quality = 3 }, //will try to get -4
                new Item{Name = "Conjured Item-that-must-not-degrade-past-zero", SellIn = 1, Quality = 1 }, //will try to get -2

            };

            Program app = new Program { Items = Items };
            app.UpdateQuality();

            Assert.AreEqual(app.Items[0].Quality, 8);
            Assert.AreEqual(app.Items[1].Quality, 16);
            Assert.AreEqual(app.Items[2].Quality, 8);
            Assert.AreEqual(app.Items[3].Quality, 8);
            Assert.AreEqual(app.Items[4].Quality, 0);
            Assert.AreEqual(app.Items[5].Quality, 0);

        }        
     

    }
}
