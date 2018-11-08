using System;
using System.Collections.Generic;
using System.Linq;
using Abbrs = System.Collections.Generic.IDictionary<string, string>;
using Items = System.Collections.Generic.IDictionary<string, object>;
using ItemsEnumerable = System.Collections.Generic.IEnumerable<System.Collections.Generic.IDictionary<string, object>>;

namespace Mappy
{
    public class Mappy
    {
        private MappyOptions _options { get; set; }

        public Mappy(MappyOptions options = null)
        {
            _options = options ?? MappyOptions.Default;
        }

        public IEnumerable<T> Map<T>(
            IEnumerable<dynamic> items)
        {
            return Map<T>(items, _options);
        }

        public IEnumerable<T> Map<T>(
            IEnumerable<dynamic> items,
            MappyOptions options)
        {
            return Map<T>(items, options, null);
        }

        public IEnumerable<T> Map<T>(
            IEnumerable<dynamic> items,
            MappyOptions options,
            Abbrs abbreviations)
        {
            return MapInternal<T>(items, options, abbreviations);
        }

        public T Map<T>(
            Items items)
        {
            return Map<T>(items, _options);
        }

        public T Map<T>(
            Items items,
            MappyOptions options)
        {
            return Map<T>(items, options, null);
        }

        public T Map<T>(
            Items items,
            MappyOptions options,
            Abbrs abbreviations)
        {
            if (items == null)
            {
                throw new ArgumentException("Mapping value should be specified.");
            }
            return MapInternal<T>(new List<Items> { items },
                options, abbreviations)
                .SingleOrDefault();
        }

        public IEnumerable<T> Map<T>(
            ItemsEnumerable items)
        {
            return Map<T>(items, _options);
        }

        public IEnumerable<T> Map<T>(
            ItemsEnumerable items,
            MappyOptions options)
        {
            return Map<T>(items, options, null);
        }

        public IEnumerable<T> Map<T>(
            ItemsEnumerable items,
            MappyOptions options,
            Abbrs abbreviations)
        {
            return MapInternal<T>(items, options, abbreviations);
        }

        internal IEnumerable<T> MapInternal<T>(
            IEnumerable<dynamic> items,
            MappyOptions options,
            Abbrs abbreviations)
        {
            var dictionary = items
                .Select(dynamicItem => dynamicItem as IDictionary<string, object>)
                .Where(x => x != null);
            return MapInternal<T>(dictionary, options, abbreviations);

        }

        internal IEnumerable<T> MapInternal<T>(
            ItemsEnumerable items,
            MappyOptions options,
            Abbrs abbreviations)
        {
            if (_options == null)
            {
                _options = MappyOptions.Default;
            }

            if (options == null)
            {
                options = _options;
            }

            if (!options.UseDefaultDictionaryComparer)
            {
                items = items
                    .Select(dynamicItem => new Dictionary<string, object>(dynamicItem, options.StringComparer));
            }

            var context = new MappingContext(options);

            return options.Cache.GetOrCreateTypeMap<T>(options)
                .MapEnumerable(items, null, context);
        }
    }
}
