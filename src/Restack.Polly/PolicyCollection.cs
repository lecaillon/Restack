﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;

namespace Polly
{
    public class PolicyCollection : Collection<Policy<HttpResponseMessage>>
    {
        private Policy<HttpResponseMessage> _cached;

        public Policy<HttpResponseMessage> Combine()
        {
            if (_cached == null)
            {
                int nbItems = Items?.Count ?? 0;
                if(nbItems > 0)
                {
                    _cached = nbItems > 1
                        ? Policy.WrapAsync(Items.ToArray())
                        : Items[0];
                }
            }

            return _cached;
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            _cached = null;
        }

        protected override void InsertItem(int index, Policy<HttpResponseMessage> item)
        {
            base.InsertItem(index, item);
            _cached = null;
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            _cached = null;
        }

        protected override void SetItem(int index, Policy<HttpResponseMessage> item)
        {
            base.SetItem(index, item);
            _cached = null;
        }
    }
}
