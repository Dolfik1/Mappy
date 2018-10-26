using System.Collections.Generic;
using System.Linq;
using Abbrs = System.Collections.Generic.IDictionary<string, string>;
using Items = System.Collections.Generic.IDictionary<string, object>;
using ItemsEnumerable = System.Collections.Generic.IEnumerable<System.Collections.Generic.IDictionary<string, object>>;

namespace Mappy
{
    public struct Mappy
    {
        private MappyOptions _options { get; set; }

        public Mappy(MappyOptions options)
        {
            _options = options;
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

            return MapInternal<T>(new List<Items> { items },
                options, abbreviations)
                .SingleOrDefault();
        }

        internal IEnumerable<T> MapInternal<T>(
            IEnumerable<dynamic> items,
            MappyOptions options,
            Abbrs abbreviations)
        {
            var dictionary = items
                .Select(dynamicItem => dynamicItem as IDictionary<string, object>)
                .ToList();

            return MapInternal<T>(dictionary, options, abbreviations);

        }

        internal IEnumerable<T> MapInternal<T>(
            ItemsEnumerable items,
            MappyOptions options,
            Abbrs abbreviations)
        {
            var def = default(MappyOptions);
            if (_options.Equals(def))
            {
                _options = MappyOptions.Default;
            }

            if (options.Equals(def))
            {
                options = _options;
            }

            return options.Cache.GetOrCreateTypeMap<T>(options)
                .MapEnumerable(items);
        }
    }
}
