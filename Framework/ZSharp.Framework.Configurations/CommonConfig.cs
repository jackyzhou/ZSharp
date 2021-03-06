﻿using System.Configuration;
using ZSharp.Framework.Extensions;

namespace ZSharp.Framework.Configurations
{
    public class CommonConfig
    {
        /// <summary>
        /// 是否支持MongoDb
        /// </summary>
        public static bool IsSupportMongoDb
        {
            get
            {
                var isSupport = false;
                var isSupportMongoStr = ConfigurationManager.AppSettings["IsSupportMongoDb"];
                if (!isSupportMongoStr.IsNullOrEmpty())
                {
                    isSupport = isSupportMongoStr.ToBool(false);
                }

                return isSupport;
            }
        }

        /// <summary>
        /// 額外支持的序列化方式：Jil, MsgPack, ProtoBuf, All
        /// </summary>
        public static string SerializationFormatType
        {
            get
            {
                return ConfigurationManager.AppSettings["SerializationFormatType"];
            }
        }

        /// <summary>
        /// 所屬程序系統代碼
        /// </summary>
        public static string SystemCode
        {
            get
            {
                return ConfigurationManager.AppSettings["SystemCode"];
            }
        }

        /// <summary>
        /// LRUCache定期清理數據時間間隔,單位秒,默認5s清理一次
        /// </summary>
        public static int LRUCacheCleaningIntervalSeconds
        {
            get
            {
                return ConfigurationManager.AppSettings["LRUCacheCleaningIntervalSeconds"].ToInt(5);
            }
        }

        /// <summary>
        /// LRUCache存儲最大限制，默認10000條
        /// </summary>
        public static int LRUCacheMaxSize
        {
            get
            {
                return ConfigurationManager.AppSettings["LRUCacheMaxSize"].ToInt(10000);
            }
        }

        /// <summary>
        /// 連接Redis集群所使用的名稱
        /// </summary>
        public static string RedisConfigName
        {
            get
            {
                return ConfigurationManager.AppSettings["RedisConfigName"];
            }
        }

        /// <summary>
        /// 連接RabbitMq集群所使用的名稱
        /// </summary>
        public static string RabbitMqConfigName
        {
            get
            {
                return ConfigurationManager.AppSettings["RabbitMqConfigName"];
            }
        }

        public static string EngineTypeSetting
        {
            get
            {
                return ConfigurationManager.AppSettings["sm:EngineType"];
            }
        }

        public static bool EnableDynamicDiscovery
        {
            get
            {
                var isEnable = false;
                var isEnableStr = ConfigurationManager.AppSettings["sm:EnableDynamicDiscovery"];
                if (!isEnableStr.IsNullOrEmpty())
                {
                    isEnable = isEnableStr.ToBool(false);
                }

                return isEnable;
            }
        }

        public static bool ClearPluginsShadowDirectoryOnStartup
        {
            get
            {
                var isClear = true;
                var isClearStr = ConfigurationManager.AppSettings["sm:ClearPluginsShadowDirectoryOnStartup"];
                if (!isClearStr.IsNullOrEmpty())
                {
                    isClear = isClearStr.ToBool(true);
                }

                return isClear;
            }
        }
    }
}
