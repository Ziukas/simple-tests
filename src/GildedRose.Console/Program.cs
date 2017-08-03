using System;
using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        public IList<Item> Items;
        static void Main(string[] args)
        {
            System.Console.WriteLine("Started");

            var app = new Program()
                          {
                              Items = new List<Item>
                                          {
                                              new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                              new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                              new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                              new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                              new Item
                                                  {
                                                      Name = "Backstage passes to a TAFKAL80ETC concert",
                                                      SellIn = 15,
                                                      Quality = 20
                                                  },
                                              new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                          }

                          };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void ChangeIItemQualityByJ(int i, int J)
        {
            if (J == 0) return;
            if (J > 0)
                Items[i].Quality = Math.Min(50, Items[i].Quality + J);
            else
                Items[i].Quality = Math.Max(0, Items[i].Quality + J);
        }

        public void UpdateQuality()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name != "Sulfuras, Hand of Ragnaros")
                {
                    //Hand of Ragnaros stays as is, all others have their SellIn decreased by one
                    Items[i].SellIn--;
                    int adjustBy = 0;
                    switch (Items[i].Name)
                    {                        
                        case "Aged Brie":
                            adjustBy = Items[i].SellIn < 0 ? 2 : 1; //increases in Quality as it's SellIn value approaches, double when after
                            break;                        
                        case "Backstage passes to a TAFKAL80ETC concert":
                            if (Items[i].SellIn < 0)  //after concert, the pass quality is set to 0
                                Items[i].Quality = 0;
                            else if (Items[i].SellIn < 5) //when there are 5 days or less to the concert, quality increases by 3
                                adjustBy = 3;
                            else if (Items[i].SellIn < 10) //Quality increases by 2 when there are 10 days or less
                                adjustBy = 2;
                            else //increases in Quality as it's SellIn value approaches
                                adjustBy = 1;
                            break;
                        default:
                            if (Items[i].Name.Substring(0, 8) == "Conjured")
                                adjustBy = Items[i].SellIn < 0 ? -4 : -2; //"Conjured" items degrade in Quality twice as fast as normal items, double when after SellIn
                            else
                                adjustBy = Items[i].SellIn < 0 ? -2 : -1; //At the end of each day our system lowers both values for every item - Once the sell by date has passed, Quality degrades twice as fast
                            break;
                    }
                    ChangeIItemQualityByJ(i, adjustBy);
                }
                
            }
        }

    }

    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

}
