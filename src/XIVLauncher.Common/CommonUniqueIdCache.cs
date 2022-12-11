using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using XIVLauncher.Common.PlatformAbstractions;

namespace XIVLauncher.PlatformAbstractions
{
    public class CommonUniqueIdCache : IUniqueIdCache
    {
        private const int DAYS_TO_TIMEOUT = 1;

        private List<UniqueIdCacheEntry> _cache;

        public CommonUniqueIdCache(FileInfo saveFile)
        {
            this.configFile = saveFile;

            Load();
        }

        #region SaveLoad

        private readonly FileInfo configFile;

        public void Save() =>
            File.WriteAllText(configFile.FullName, JsonSerializer.Serialize(_cache, new JsonSerializerOptions { WriteIndented = true }));

        public void Load()
        {
            if (!File.Exists(configFile.FullName))
            {
                _cache = new List<UniqueIdCacheEntry>();
                return;
            }

            _cache = JsonSerializer.Deserialize<List<UniqueIdCacheEntry>>(File.ReadAllText(configFile.FullName)) ?? new List<UniqueIdCacheEntry>();
        }

        public void Reset()
        {
            _cache.Clear();
            Save();
        }

        #endregion

        private void DeleteOldCaches()
        {
            _cache.RemoveAll(entry => (DateTime.Now - entry.CreationDate).TotalDays > DAYS_TO_TIMEOUT);
        }

        public bool HasValidCache(string userName)
        {
            return _cache.Any(entry => IsValidCache(entry, userName));
        }

        public void Add(string userName, string uid, int region, int expansionLevel)
        {
            _cache.Add(new UniqueIdCacheEntry
            {
                CreationDate = DateTime.Now,
                UserName = userName,
                UniqueId = uid,
                Region = region,
                ExpansionLevel = expansionLevel
            });

            Save();
        }

        public bool TryGet(string userName, out IUniqueIdCache.CachedUid cached)
        {
            DeleteOldCaches();

            var cache = _cache.FirstOrDefault(entry => IsValidCache(entry, userName));

            if (cache == null)
            {
                cached = default;
                return false;
            }

            cached = new IUniqueIdCache.CachedUid
            {
                UniqueId = cache.UniqueId,
                Region = cache.Region,
                MaxExpansion = cache.ExpansionLevel,
            };
            return true;
        }

        private bool IsValidCache(UniqueIdCacheEntry entry, string name) => entry.UserName == name &&
                                                                            (DateTime.Now - entry.CreationDate).TotalDays <=
                                                                            DAYS_TO_TIMEOUT;

        public class UniqueIdCacheEntry
        {
            public string UserName { get; set; }
            public string UniqueId { get; set; }
            public int Region { get; set; }
            public int ExpansionLevel { get; set; }

            public DateTime CreationDate { get; set; }
        }
    }
}