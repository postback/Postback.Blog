using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Postback.Blog.Models.ViewModels
{
    public class PagingView
    {
        public IList<PagingItem> Items
        {
            get { return items ?? (items = GetItems()); }
        }
        private const int NUMBER_OF_PAGES_TO_SHOW = 9;
        private IList<PagingItem> items;
        public int ItemsOnOnePage { get; set; }
        public int ItemCount { get; set; }
        public int CurrentPage { get; set; }
        public Func<string> Uri = () => string.Empty;

        public IList<PagingItem> GetItems()
        {
            var list = new List<PagingItem>();
            var division = Convert.ToDouble(ItemCount) / Convert.ToDouble(ItemsOnOnePage);
            var pagesCount = Convert.ToInt32(Math.Ceiling(division));

            if(pagesCount > 1)
            {
                //First page, anyway
                list.Add(new PagingItem { Label = "1", Uri = Uri(), Selected = CurrentPage == 1 });

                //Define the range in the middle
                var startAt = 2;
                int endAt;
                if (NUMBER_OF_PAGES_TO_SHOW >= pagesCount)
                {
                    endAt = pagesCount - 1;
                }
                else
                {
                    //CurrentPage
                    startAt = CurrentPage - (NUMBER_OF_PAGES_TO_SHOW/2);
                    if(startAt < 2)
                    {
                        startAt = 2;
                    }
                    endAt = startAt + (NUMBER_OF_PAGES_TO_SHOW - 1);
                }

                //Add the range
                for (int i = startAt; i <= endAt; i++)
                {
                    list.Add(new PagingItem { Label = i.ToString(), Uri = Uri() + "?page=" + i, Selected = CurrentPage == i });
                }

                //Last page, anyway
                list.Add(new PagingItem { Label = pagesCount.ToString(), Uri = Uri() + "?page=" + pagesCount, Selected = CurrentPage == pagesCount });
            }

            return list;
        }
    }

    public class PagingItem
    {
        public string Uri { get; set; }
        public string Label { get; set; }
        public bool Selected { get; set; }
    }
}